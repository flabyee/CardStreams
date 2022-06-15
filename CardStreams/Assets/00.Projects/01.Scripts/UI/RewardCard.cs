using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public void Init(SpecialCardSO so)
    {
        cover.SetActive(true);
        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;

        cardSO = so;
    }

    public void StatInit(int goldValue, bool allHeal) // ī���ε� �����ִ� / �Ǹ�ȸ����Ű�� ī��� StatInit
    {
        getGoldAmount = goldValue;
        getHeal = allHeal;

        //rewardImage.sprite = so.sprite;
        //rewardNameText.text = so.specialCardName;  ���߿��̰� ��/�� �̹����� ��ü
    }

    public void PressButton()
    {
        if (_isget) return;

        _isget = true;
        cover.SetActive(false);

        if (getGoldAmount > 0)
        {
            // Bezier�� ��UI�� ������ �����ϸ� ������
        }
        else if(getHeal == true)
        {
            // Bezier�� ȸ������ ������ �����ϸ� ȸ��
        }
        else
        {
            EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, cardSO.id, CardType.Special);
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
