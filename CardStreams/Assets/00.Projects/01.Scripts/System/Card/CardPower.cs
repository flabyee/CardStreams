using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardPower : MonoBehaviour
{
    public Image faceImage;
    public Image backImage;
    public TextMeshProUGUI valueText;

    [Header("Debug")]
    public DropAreaType dropAreaType;
    public CardType cardType;
    public int value;
    public int originValue;


    public void SetData_Feild(CardType cardType, int value)
    {
        this.dropAreaType = DropAreaType.feild;
        this.cardType = cardType;
        this.value = value;
        this.originValue = value;

        switch (cardType)
        {
            case CardType.Potion:
                faceImage.sprite = ConstManager.Instance.potionSprite;
                break;
            case CardType.Sword:
                faceImage.sprite = ConstManager.Instance.swordSprite;
                break;
            case CardType.Sheild:
                faceImage.sprite = ConstManager.Instance.sheildSprite;
                break;
            case CardType.Monster:
                faceImage.sprite = ConstManager.Instance.monsterSprite;
                break;
            case CardType.Coin:
                faceImage.sprite = ConstManager.Instance.coinSprite;
                break;
        }
        valueText.text = value.ToString();
    }

    public void SetData_SpecialCard()
    {
        this.dropAreaType = DropAreaType.feild;
        this.cardType = CardType.Special;
    }

    public void SetData_Build()
    {
        this.dropAreaType = DropAreaType.build;
        this.cardType = CardType.Build;
    }

    public void ApplyUI()
    {
        switch (cardType)
        {
            case CardType.Potion:
                faceImage.sprite = ConstManager.Instance.potionSprite;
                break;
            case CardType.Sword:
                faceImage.sprite = ConstManager.Instance.swordSprite;
                break;
            case CardType.Sheild:
                faceImage.sprite = ConstManager.Instance.sheildSprite;
                break;
            case CardType.Monster:
                faceImage.sprite = ConstManager.Instance.monsterSprite;
                break;
            case CardType.Coin:
                faceImage.sprite = ConstManager.Instance.coinSprite;
                break;
        }

        valueText.text = value.ToString();
        
        if(value == originValue)
        {
            valueText.color = Color.white;
        }
        else if(value > originValue)
        {
            valueText.color = Color.blue;
        }
        else if(value < originValue)
        {
            valueText.color = Color.red;
        }
    }

    public void AddValue(int value)
    {
        this.value += value;
        ApplyUI();
    }

    public void SetValue(int value)
    {
        this.value = value;
        ApplyUI();
    }


}
