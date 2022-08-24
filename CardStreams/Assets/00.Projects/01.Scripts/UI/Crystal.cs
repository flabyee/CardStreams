using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public TextMeshProUGUI crystalText;

    public static int crystalAmount = 0;

    private void Start()
    {
        // �ε�
        
        crystalAmount = SaveFile.recentFile.gold;
        crystalText.text = crystalAmount.ToString();
    }

    public void ApplyCrystalToText()
    {
        crystalText.text = crystalAmount.ToString();

        // ����
        SaveFile.recentFile.gold = crystalAmount;
    }
}
