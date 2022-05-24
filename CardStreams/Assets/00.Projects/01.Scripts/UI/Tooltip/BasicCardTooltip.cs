using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicCardTooltip : MonoBehaviour
{
    public static BasicCardTooltip Instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI valueText;

    public Image cardImage;

    // 버프 관련

    private void Awake()
    {
        Instance = this;    
    }

    public void Show(CardPower cardPower)
    {
        typeText.text = cardPower.cardType.ToString();
        valueText.text = cardPower.value.ToString();

        cardImage.sprite = cardPower.faceImage.sprite;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
