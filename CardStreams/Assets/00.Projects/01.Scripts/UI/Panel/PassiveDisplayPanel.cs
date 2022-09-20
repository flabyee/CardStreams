using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveDisplayPanel : MonoBehaviour
{
    [SerializeField] List<PassiveSlot> passiveSlots = new List<PassiveSlot>();

    private int slotCount;

    private void Start()
    {
        for (int i = 0; i < passiveSlots.Count; i++)
        {
            passiveSlots[i].gameObject.SetActive(false); // 끄고 있다가 버프들어오면 on
        }
    }

    public void SetPassive(Passive passive) // 일단 개대충만듬 3번 호출 ㄱ
    {
        passiveSlots[slotCount].Init(passive.buffIcon, passive.buffName, passive.currentLevel);
        passiveSlots[slotCount].gameObject.SetActive(true);

        slotCount++;
    }
}
