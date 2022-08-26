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
        // 로드
        
        crystalAmount = SaveFile.GetSaveData().gold;
        crystalText.text = crystalAmount.ToString();
    }

    public void ApplyCrystalToText()
    {
        crystalText.text = crystalAmount.ToString();

        // 저장
        SaveFile.GetSaveData().gold = crystalAmount;
    }
}
