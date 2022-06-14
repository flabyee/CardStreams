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

    public void Init(SpecialCardSO so)
    {
        cover.SetActive(true);
        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;

        cardSO = so;
    }

    public void PressButton()
    {
        if (_isget) return;

        _isget = true;
        cover.SetActive(false);


        EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, cardSO.id, CardType.Special);
    }

    public void ResetReward()
    {
        gameObject.SetActive(false);
        this.cardSO = null;
        rewardImage.sprite = null;
        rewardNameText.text = null;
    }
}
