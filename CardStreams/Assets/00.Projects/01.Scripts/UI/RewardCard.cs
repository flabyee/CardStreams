using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public enum TargetType // ����ī�尡 ���ư� ��ġ
{
    Bag,
    GoldUI,
    HPUI,
    Exp,
}

public class RewardCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject cover; // ī�� �������� �ո�

    [Header("���� ī�� �Ӽ�")]
    [SerializeField] Image rewardImage; // ī���� �̹���
    [SerializeField] TextMeshProUGUI rewardNameText; // ī���� �̸�
    [SerializeField] TextMeshProUGUI rewardDescriptionText; // ī���� ����


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

    public bool isGet = false; // ī���� ������ �������?
    
    private float reverseTime = 1f; // ī�尡 ���������� �ð�
    private bool isCompleteReverse = false; // ī�尡 �������� ���� �ո��� ����Ǿ�����?


    private SpecialCardSO cardSO;
    
    private void DefaultInit(float cardReverseTime) // ī�� ������ ���ֱ�(���۽ÿ����)
    {
        cover.SetActive(true);
        gameObject.SetActive(true);
        reverseTime = cardReverseTime;
        isGet = false;
        isCompleteReverse = false;
    }

    public void CardInit(SpecialCardSO so, float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;
        rewardDescriptionText.text = so.tooltip;

        Debug.Log(so == null);
        cardSO = so;
    }

    public void GoldInit(int goldValue, float cardReverseTime) // ī���ε� �����ִ� / �Ǹ�ȸ����Ű�� ī��� Gold Init / Heal Init
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = goldSprite;
        rewardNameText.text = "��";
        rewardDescriptionText.text = $"�� {goldValue}�� ����ϴ�.";
        getGoldAmount = goldValue;
    }

    public void HealInit(float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = healSprite;
        rewardNameText.text = "ȸ��";
        rewardDescriptionText.text = "ü���� ���� ȸ���մϴ�.";
        getHeal = true;
    }

    public void PressButton() // called by Button or RewardManager OK Button
    {
        if (isGet) return;

        isGet = true;
        SoundManager.Instance.PlaySFX(SFXType.CardReverse, 0.3f);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(0, reverseTime / 2)); // ī�� 1/2 ������
        seq.AppendCallback(() => cover.gameObject.SetActive(false)); // �޺κ� ����
        seq.Append(transform.DOScaleX(1, reverseTime / 2)); // ī�� ������ ������
        seq.AppendCallback(() => isCompleteReverse = true); // �ٵ��������� ���� true
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
            EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, TargetType.Bag, () => { });
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
        rewardDescriptionText.text = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isCompleteReverse)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isCompleteReverse)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
