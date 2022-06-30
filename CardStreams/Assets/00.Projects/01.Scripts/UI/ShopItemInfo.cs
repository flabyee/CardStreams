using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardType cardType;

    public string itemName;
    public List<CardType> targetTypeList;
    public List<BasicType> targetBasicList;
    public List<Vector2> accessPointList;
    public string tooltip;
    public int price;
    public Image itemImage;
    public Image gradeImage;
    public TextMeshProUGUI priceText;

    public Button button;

    public void Init(string itemName, List<CardType> targetTypeList, List<BasicType> targetBasicList, string tooltip, Sprite sprite, CardGrade grade, int price)
    {
        cardType = CardType.Special;

        this.itemName = itemName;
        this.targetTypeList = targetTypeList;
        this.targetBasicList = targetBasicList;
        this.tooltip = tooltip;
        itemImage.sprite = sprite;
        gradeImage.color = ConstManager.Instance.gradeColorDict[(int)grade];
        this.price = price;
        priceText.text = price.ToString();
    }

    public void Init(string itemName, List<Vector2> accessPointList, string tooltip, Sprite sprite, CardGrade grade, int price)
    {
        cardType = CardType.Build;

        this.itemName = itemName;
        this.accessPointList = accessPointList;
        this.tooltip = tooltip;
        itemImage.sprite = sprite;
        gradeImage.color = ConstManager.Instance.gradeColorDict[(int)grade];
        this.price = price;
        priceText.text = price.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch(cardType)
        {
            case CardType.Special:
                SpecialCardTooltip.Instance.Show(itemName, targetTypeList, targetBasicList, tooltip, itemImage.sprite, transform.position);

                break;
            case CardType.Build:
                BuildTooltip.Instance.Show(itemName, accessPointList, tooltip, itemImage.sprite, transform.position);

                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (cardType)
        {
            case CardType.Special:
                SpecialCardTooltip.Instance.Hide();

                break;
            case CardType.Build:
                BuildTooltip.Instance.Hide();

                break;
        }
    }
}
