using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageRewardSlot : MonoBehaviour
{
    public Image slotImage;
    public TextMeshProUGUI rewardNameText;
    public TextMeshProUGUI countText;
    private int rewardCount;

    public void Init(VillageRewardSO so) // 슬롯 초기화
    {
        rewardCount = 1;
        slotImage.sprite = so.rewardSprite;
        rewardNameText.text = so.rewardName;
        countText.text = $"x{rewardCount}";
    }

    public void AddCount() // 보상 수 +1
    {
        rewardCount++;
        countText.text = $"x{rewardCount}";
    }
}