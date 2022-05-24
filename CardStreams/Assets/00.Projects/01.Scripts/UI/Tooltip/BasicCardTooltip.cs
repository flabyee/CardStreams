using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicCardTooltip : MonoBehaviour
{
    public static BasicCardTooltip Instance;

    public TextMeshProUGUI typeText;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI originValueText;

    public Image cardImage;

    // 버프 관련

    private void Awake()
    {
        Instance = this;    
    }

    public void Show(CardPower cardPower, Vector3 pos)
    {
        transform.position = pos;

        typeText.text = cardPower.cardType.ToString();
        valueText.text = cardPower.value.ToString();
        originValueText.text = cardPower.originValue.ToString();

        if (cardPower.value == cardPower.originValue)
        {
            valueText.color = Color.white;
        }
        else if (cardPower.value > cardPower.originValue)
        {
            valueText.color = Color.blue;
        }
        else if (cardPower.value < cardPower.originValue)
        {
            valueText.color = Color.red;
        }

        cardImage.sprite = cardPower.faceImage.sprite;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
