using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageRewardPanel : MonoBehaviour
{
    private Dictionary<VillageRewardSO, VillageRewardSlot> rewardSlotDict = new Dictionary<VillageRewardSO, VillageRewardSlot>();

    [SerializeField] List<VillageRewardSlot> slots;
    private int slotCount = 0;
    private CanvasGroup cg;
    private bool isShow;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public void OnOffPanel()
    {
        if(isShow) // on -> off
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
        else // off -> on
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        isShow = !isShow;
    }

    public void AddReward(VillageRewardSO so) // 보상먹은거 추가
    {
        if(rewardSlotDict.ContainsKey(so)) // 먹었으면 +1
        {
            rewardSlotDict[so].AddCount();
        }
        else // 안먹었으면 추가
        {
            VillageRewardSlot slot = slots[slotCount];
            Debug.Log("추가");

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