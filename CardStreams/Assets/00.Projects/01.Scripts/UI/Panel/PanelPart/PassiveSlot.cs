using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSlot : MonoBehaviour
{
    [SerializeField] Image passiveImage;
    [SerializeField] TextMeshProUGUI passiveLevelText;
    [SerializeField] TextMeshProUGUI passiveNameText;

    private const string levelText = "Lv";

    public void Init(Sprite passiveSprite)
    {
        passiveImage.sprite = passiveSprite;
        passiveLevelText.text = 1 + levelText;
    }

    public void Init(Sprite icon, string passiveName, int level)
    {
        passiveImage.sprite = icon;
        passiveLevelText.text = passiveName;
        passiveNameText.text = level + levelText;
    }

    public void LevelUpdate(int level)
    {
        passiveLevelText.text = level + levelText;
    }

    // (PassiveSO라는거 새로 만들어서 레벨에따른 BuffSO 어쩌고저쩌고orBuffSO 레벨수치
    // 패시브 건물도 추가하기
}
