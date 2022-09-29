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

    public void Init(VillageRewardSO so) // ���� �ʱ�ȭ
    {
        rewardCount = 1;
        slotImage.sprite = so.rewardSprite;
        rewardNameText.text = so.rewardName;
        countText.text = $"x{rewardCount}";
    }

    public void AddCount() // ���� �� +1
    {
        rewardCount++;
        countText.text = $"x{rewardCount}";
    }
}