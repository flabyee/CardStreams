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
        // 로드

        prestigeAmount = SaveFile.GetSaveData().prestige;
        prestigeText.text = prestigeAmount.ToString();
    }

    public void ApplyPrestigeToText() // 이 스크립트가 있는곳의 Event Listener에서 호출됨
    {
        prestigeText.text = prestigeAmount.ToString();

        // 저장
        SaveFile.GetSaveData().prestige = prestigeAmount;
    }
}
