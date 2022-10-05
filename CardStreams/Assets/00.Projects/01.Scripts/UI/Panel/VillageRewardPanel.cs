using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageRewardPanel : Panel
{
    private Dictionary<VillageRewardSO, VillageRewardSlot> rewardSlotDict = new Dictionary<VillageRewardSO, VillageRewardSlot>();

    [SerializeField] List<VillageRewardSlot> slots;
    private int slotCount = 0;

    public void AddReward(VillageRewardSO so) // ��������� �߰�
    {
        if(rewardSlotDict.ContainsKey(so)) // �Ծ����� +1
        {
            rewardSlotDict[so].AddCount();
        }
        else // �ȸԾ����� �߰�
        {
            VillageRewardSlot slot = slots[slotCount];

            slot.Init(so);
            slot.gameObject.SetActive(true);
            rewardSlotDict.Add(so, slot);

            if(slotCount >= 7)
            {
                Debug.Log("�̸� ������ ������ ������ �� ������ ������");
            }
            slotCount++;
        }
    }
}