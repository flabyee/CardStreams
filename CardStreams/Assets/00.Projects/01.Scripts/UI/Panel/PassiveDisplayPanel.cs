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
            passiveSlots[i].gameObject.SetActive(false); // ���� �ִٰ� ���������� on
        }
    }

    public void SetPassive(Passive passive) // �ϴ� �����游�� 3�� ȣ�� ��
    {
        passiveSlots[slotCount].Init(passive.buffIcon, passive.buffName, passive.currentLevel);
        passiveSlots[slotCount].gameObject.SetActive(true);

        slotCount++;
    }
}
