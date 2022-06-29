using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum TargetType // ����ī�尡 ���ư� ��ġ
{
    Handle,
    GoldUI,
    HPUI
}

public class RewardCard : MonoBehaviour
{
    [SerializeField] GameObject cover; // ī�� �������� �ո�

    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI rewardNameText;

    private bool _isget = false;
    private SpecialCardSO cardSO;

    [Header("�׽�Ʈ : �ܼ� ī���϶��� ���� ���")]
    [SerializeField] private int getGoldAmount = 0;
    [SerializeField] private bool getHeal = false;

    [SerializeField] float reverseTime = 1f;

    [Header("�ܼ� ���� ����")]
    [SerializeField] Sprite healSprite;
    [SerializeField] Sprite goldSprite;

    [Header("�ܼ� ���� �������� SO")]
    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;
    [SerializeField] EventSO goldValueChanged;
    [SerializeField] EventSO playerValueChanged;
    
    private void DefaultInit() // ī�� ������ ���ֱ�(���۽ÿ����)
    {
        cover.SetActive(true);
        gameObject.SetActive(true);
        _isget = false;
    }

    public void CardInit(SpecialCardSO so)
    {
        DefaultInit();

        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;

        cardSO = so;
    }

    public void GoldInit(int goldValue) // ī���ε� �����ִ� / �Ǹ�ȸ����Ű�� ī��� Gold Init / Heal Init
    {
        DefaultInit();

        rewardImage.sprite = goldSprite;
        rewardNameText.text = "��";
        getGoldAmount = goldValue;
    }

    public void HealInit()
    {
        DefaultInit();

        rewardImage.sprite = healSprite;
        rewardNameText.text = "ȸ��";
        getHeal = true;
    }

    public void PressButton()
    {
        if (_isget) return;

        _isget = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(0, reverseTime / 2)); // ī�� 1/2 ������
        seq.AppendCallback(() => cover.gameObject.SetActive(false)); // �޺κ� ����
        seq.Append(transform.DOScaleX(1, reverseTime / 2)); // ī�� ������ ������
    }

    public void GetReward()
    {
        if (getGoldAmount > 0)
        {
            // Bezier�� ��UI�� ������ �����ϸ� ������
            EffectManager.Instance.GetBezierCardEffect(transform.position, goldSprite, TargetType.GoldUI, () =>
            {
                goldValue.RuntimeValue += getGoldAmount;
                goldValueChanged.Occurred();
            });
        }
        else if (getHeal == true)
        {
            // Bezier�� ȸ������ ������ �����ϸ� ȸ��
            EffectManager.Instance.GetBezierCardEffect(transform.position, healSprite, TargetType.HPUI, () =>
            {
                hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
                playerValueChanged.Occurred();
            });
        }
        else
        {
            EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, TargetType.Handle, () => { });
            GameManager.Instance.handleController.AddSpecial(cardSO.id);
        }
    }

    public void ResetReward() // ī�� �Ȼ����� or �ٻ̾����� ���ֱ�
    {
        gameObject.SetActive(false);
        transform.rotation = Quaternion.Euler(Vector3.zero); // ī�� �����°� �ʱ�ȭ
        this.cardSO = null;
        rewardImage.sprite = null;
        rewardNameText.text = null;
    }
}
