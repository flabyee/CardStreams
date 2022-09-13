using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Prestige : MonoBehaviour
{
    public TextMeshProUGUI prestigeText;

    public static int prestigeAmount = 0;

    private void Start()
    {
        // �ε�

        prestigeAmount = SaveFile.GetSaveData().prestige;
        prestigeText.text = prestigeAmount.ToString();
    }

    public void ApplyPrestigeToText() // �� ��ũ��Ʈ�� �ִ°��� Event Listener���� ȣ���
    {
        prestigeText.text = prestigeAmount.ToString();

        // ����
        SaveFile.GetSaveData().prestige = prestigeAmount;
    }
}
