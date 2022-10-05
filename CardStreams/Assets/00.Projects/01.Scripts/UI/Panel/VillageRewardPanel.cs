using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageRewardPanel : Panel
{
    private Dictionary<VillageRewardSO, VillageRewardSlot> rewardSlotDict = new Dictionary<VillageRewardSO, VillageRewardSlot>();

    [SerializeField] List<VillageRewardSlot> slots;
    private int slotCount = 0;

    public void AddReward(VillageRewardSO so) // 보상먹은거 추가
    {
        if(rewardSlotDict.ContainsKey(so)) // 먹었으면 +1
        {
            rewardSlotDict[so].AddCount();
        }
        else // 안먹었으면 추가
        {
            VillageRewardSlot slot = slots[slotCount];

            slot.Init(so);
            slot.gameObject.SetActive(true);
            rewardSlotDict.Add(so, slot);

            if(slotCount >= 7)
            {
                Debug.Log("미리 만들어둔 슬롯이 꽉차서 새 보상은 에러뜸");
            }
            slotCount++;
        }
    }
}