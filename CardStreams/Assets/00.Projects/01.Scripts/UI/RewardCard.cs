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


    [Header("�׽�Ʈ : �ܼ� ī���϶��� ���� ���")]
    [SerializeField] int getGoldAmount = 0;
    [SerializeField] bool getHeal = false;


    [Header("�ܼ� ���� ����")]
    [SerializeField] Sprite healSprite;
    [SerializeField] Sprite goldSprite;

    [Header("�ܼ� ���� �������� SO")]
    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;
    [SerializeField] EventSO goldValueChanged;
    [SerializeField] EventSO playerValueChanged;

    
    private float reverseTime = 1f;
    public bool isget = false;


    private SpecialCardSO cardSO;
    
    private void DefaultInit(float cardReverseTime) // ī�� ������ ���ֱ�(���۽ÿ����)
    {
        cover.SetActive(true);
        gameObject.SetActive(true);
        reverseTime = cardReverseTime;
        isget = false;
    }

    public void CardInit(SpecialCardSO so, float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;

        Debug.Log(so == null);
        cardSO = so;
    }

    public void GoldInit(int goldValue, float cardReverseTime) // ī���ε� �����ִ� / �Ǹ�ȸ����Ű�� ī��� Gold Init / Heal Init
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = goldSprite;
        rewardNameText.text = "��";
        getGoldAmount = goldValue;
    }

    public void HealInit(float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = healSprite;
        rewardNameText.text = "ȸ��";
        getHeal = true;
    }

    public void PressButton() // called by Button
    {
        if (isget) return;

        isget = true;
        SoundManager.Instance.PlaySFX(SFXType.CardReverse);

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
                ResetReward();
            });
        }
        else if (getHeal == true)
        {
            // Bezier�� ȸ������ ������ �����ϸ� ȸ��
            EffectManager.Instance.GetBezierCardEffect(transform.position, healSprite, TargetType.HPUI, () =>
            {
                hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
                playerValueChanged.Occurred();
                ResetReward();
            });
        }
        else
        {
            Debug.Log(cardSO);
            Debug.Log(cardSO.sprite);
            Debug.Log(cardSO.id);
            EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, TargetType.Handle, () => { });
            GameManager.Instance.handleController.AddSpecial(cardSO.id);
            ResetReward();
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
