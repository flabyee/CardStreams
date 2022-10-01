using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveDisplayPanel : MonoBehaviour
{
    [SerializeField] List<PassiveSlot> passiveSlots = new List<PassiveSlot>();

    private int slotCount;

    public void SetPassive(Passive passive) // �ϴ� �����游�� 3�� ȣ�� ��
    {
        Debug.Log("��");
        passiveSlots[slotCount].Init(passive.buffIcon, passive.buffName, passive.currentLevel);
        passiveSlots[slotCount].gameObject.SetActive(true);
        Debug.Log("as " + passiveSlots[slotCount].gameObject.activeSelf);

        slotCount++;
    }
}
