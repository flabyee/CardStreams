using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RightListUI : MonoBehaviour
{
    public Image background;
    public Image gradeImage;
    public TextMeshProUGUI nameText;
    public Button button;

    public void Init(CardType cardType, CardGrade grade, string name)
    {
        background.color = cardType == CardType.Build ?
            ConstManager.Instance.buildColor : ConstManager.Instance.specialCardColor;

        gradeImage.sprite = ConstManager.Instance.gradeSpriteDict[(int)grade];

        nameText.text = name;
    }
}
