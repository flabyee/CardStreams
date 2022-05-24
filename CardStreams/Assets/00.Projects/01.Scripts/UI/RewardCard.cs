using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI rewardNameText;
    [SerializeField] TextMeshProUGUI rewardDescriptionText;

    private RewardSO rewardSO;
    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;

    public void SelectReward() // called by Button onclick
    {
        if (rewardSO.goldReward > 0) goldValue.RuntimeValue += rewardSO.goldReward;
        if (rewardSO.allHealReward == true) hpValue.RuntimeValue = hpValue.RuntimeMaxValue;

        if (rewardSO.cardReward.Length <= 0) return;

        SaveData saveData = SaveSystem.Load();

        foreach (var cardSO in rewardSO.cardReward)
        {
            saveData.speicialCardDataList[cardSO.id].haveAmount++;
        }

        SaveSystem.Save(saveData);
    }

    public void SetReward(RewardSO so)
    {
        this.rewardSO = so;

        rewardImage.sprite = so.rewardSprite;
        rewardNameText.text = so.rewardName;
        rewardDescriptionText.text = so.rewardDescription;
    }
}
