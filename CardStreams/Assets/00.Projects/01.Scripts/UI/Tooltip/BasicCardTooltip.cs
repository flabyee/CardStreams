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

    public void Show(CardPower cardPower, Vector3 pos)
    {
        BasicCard basicPower = cardPower as BasicCard;

        transform.position = pos;

        typeText.text = basicPower.cardType.ToString();
        valueText.text = basicPower.value.ToString();

        cardImage.sprite = basicPower.faceImage.sprite;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
