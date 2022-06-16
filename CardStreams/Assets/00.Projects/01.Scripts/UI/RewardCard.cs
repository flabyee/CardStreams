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

    // �ܼ� ī���϶��� ���� ���
    private int getGoldAmount = 0;
    private bool getHeal = false;

    [Header("�ܼ� ���� ����")]
    [SerializeField] Sprite healSprite;
    [SerializeField] Sprite goldSprite;

    [Header("�ܼ� ���� �������� SO")]
    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;
    [SerializeField] EventSO goldValueChanged;
    [SerializeField] EventSO playerValueChanged;
    
    public void Init(SpecialCardSO so)
    {
        cover.SetActive(true);
        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;

        cardSO = so;
    }

    public void GoldInit(int goldValue) // ī���ε� �����ִ� / �Ǹ�ȸ����Ű�� ī��� Gold Init / Heal Init
    {
        cover.SetActive(true);
        rewardImage.sprite = goldSprite;
        rewardNameText.text = "��";
        getGoldAmount = goldValue;
    }

    public void HealInit()
    {
        cover.SetActive(true);

        rewardImage.sprite = healSprite;
        rewardNameText.text = "ȸ��";
        getHeal = true;
    }

    public void PressButton()
    {
        if (_isget) return;

        _isget = true;
        cover.SetActive(false);

        if (getGoldAmount > 0)
        {
            // Bezier�� ��UI�� ������ �����ϸ� ������
            EffectManager.Instance.GetBezierCardEffect(transform.position, goldSprite, TargetType.GoldUI, () =>
            {
                goldValue.RuntimeValue += getGoldAmount;
                goldValueChanged.Occurred();
            });
        }
        else if(getHeal == true)
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
            EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, TargetType.Handle, () => GameManager.Instance.handleController.DrawSpecialCard(cardSO.id));
        }
    }

    public void ResetReward()
    {
        gameObject.SetActive(false);
        this.cardSO = null;
        rewardImage.sprite = null;
        rewardNameText.text = null;
    }
}
