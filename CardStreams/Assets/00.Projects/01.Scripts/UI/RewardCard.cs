using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardCard : MonoBehaviour
{
    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI rewardNameText;

    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;
    [SerializeField] EventSO playerValueChanged;
    [SerializeField] EventSO goldValueChanged;

    [HideInInspector] public SelectRewardManager selectPanel;
    private RewardSO rewardSO;

    

    public void SelectReward() // called by Button onclick, 버튼누를때 작동함
    {
        // 골드 증가
        if (rewardSO.goldReward > 0)
        {
            goldValue.RuntimeValue += rewardSO.goldReward;
            goldValueChanged.Occurred();
        }

        // 체력 모두 회복
        if (rewardSO.allHealReward == true)
        {
            hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
            playerValueChanged.Occurred();
        }

        // 카드 보상이 없다면 리턴
        if (rewardSO.cardReward.Length <= 0) return;

        // 세이브로드 + 카드 소매넣기
        SaveData saveData = SaveSystem.Load();

        foreach (var cardSO in rewardSO.cardReward)
        {
            EffectManager.Instance.GetBezierCardEffect(rewardImage.sprite, rewardNameText.text);

            saveData.speicialCardDataList[cardSO.id].haveAmount++;
        }

        SaveSystem.Save(saveData);

        selectPanel.Hide(); // 버튼을 눌러 보상을 받았으니 부모 패널을 꺼요
    }

    public void SetReward(RewardSO so)
    {
        this.rewardSO = so;

        rewardImage.sprite = so.rewardSprite;
        rewardNameText.text = so.rewardName;
    }

    public void ResetReward()
    {
        this.rewardSO = null;

        rewardImage.sprite = null;
        rewardNameText.text = null;
    }
}
