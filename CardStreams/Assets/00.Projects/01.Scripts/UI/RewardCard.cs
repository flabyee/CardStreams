using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardCard : MonoBehaviour
{
    [SerializeField] GameObject cover; // 카드 누르기전 앞면

    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI rewardNameText;

    private bool _isget = false;
    private SpecialCardSO cardSO;

    // 단순 카드일때만 쓰는 멤버
    private int getGoldAmount = 0;
    private bool getHeal = false;

    public void Init(SpecialCardSO so)
    {
        cover.SetActive(true);
        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;

        cardSO = so;
    }

    public void StatInit(int goldValue, bool allHeal) // 카드인데 돈만주는 / 피만회복시키는 카드면 StatInit
    {
        getGoldAmount = goldValue;
        getHeal = allHeal;

        //rewardImage.sprite = so.sprite;
        //rewardNameText.text = so.specialCardName;  나중에이거 돈/힐 이미지로 교체
    }

    public void PressButton()
    {
        if (_isget) return;

        _isget = true;
        cover.SetActive(false);

        if (getGoldAmount > 0)
        {
            // Bezier로 돈UI로 날리기 도착하면 돈증가
        }
        else if(getHeal == true)
        {
            // Bezier로 회복으로 날리기 도착하면 회복
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
