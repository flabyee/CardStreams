using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalUI : MonoBehaviour
{
    public TextMeshProUGUI crystalText;

    int villageCrystal = 0;

    private void Awake()
    {
        SaveData data = SaveSystem.Load();
        villageCrystal = data.gold; // ÃßÈÄ Å©¸®½ºÅ»·Î ¹Ù²ã¾ßµÊ
    }

    public void ApplyCrystalToText()
    {
        crystalText.text = (GameManager.Instance.player.killMobCount * GameManager.Instance.mineLevel + villageCrystal).ToString();
    }
}
