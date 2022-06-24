using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagCard : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public Image cardImage;
    public Image gradeImage;
    public Image background;
    public CardType cardType;

    public void Init(string name, string info, Sprite sprite, CardType cardType)
    {
        nameText.text = name;
        infoText.text = info;
        cardImage.sprite = sprite;
        this.cardType = cardType;

        // 배경색 지정
    }

    public void OnClickBagCard()
    {

    }
}
