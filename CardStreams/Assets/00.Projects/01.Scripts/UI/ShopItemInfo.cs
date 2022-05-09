using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;
    public List<CardType> targetTypeList;
    public string tooltip;
    public Image itemImage;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI priceText;

    public Button button;

    public void Init(string itemName, List<CardType> targetTypeList, string tooltip, Sprite sprite, int count, int price)
    {
        this.itemName = itemName;
        this.targetTypeList = targetTypeList;
        this.tooltip = tooltip;
        itemImage.sprite = sprite;
        countText.text = count.ToString();
        priceText.text = price.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardTooltip.Instance.Show(itemName, targetTypeList, tooltip, itemImage.sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardTooltip.Instance.Hide();
    }
}
