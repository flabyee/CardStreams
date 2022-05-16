using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardPower : MonoBehaviour
{
    public Image faceImage;
    public Image backImage;
    public TextMeshProUGUI valueText;

    [Header("Debug")]
    public DropAreaType dropAreaType;
    public CardType cardType;
    public int value { get; private set; }
    private int originValue;

    // public List<BuffSO> buffList = new List<BuffSO>();
    public List<Buff> buffList = new List<Buff>();

    public void SetData_Feild(CardType cardType, int value)
    {
        this.dropAreaType = DropAreaType.feild;
        this.cardType = cardType;
        this.value = value;
        this.originValue = value;

        switch (cardType)
        {
            case CardType.Potion:
                faceImage.sprite = ConstManager.Instance.potionSprite[value];
                break;
            case CardType.Sword:
                faceImage.sprite = ConstManager.Instance.swordSprite[value];
                break;
            case CardType.Sheild:
                faceImage.sprite = ConstManager.Instance.sheildSprite[value];
                break;
            case CardType.Monster:
                faceImage.sprite = ConstManager.Instance.monsterSprite[value];
                break;
            case CardType.Coin:
                faceImage.sprite = ConstManager.Instance.coinSprite[value];
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

    public void AddBuff(Buff buff)
    {
        if (buffList.Contains(buff)) return; // 대충 중복체크해서 없으면넣기
        // 나중엔 중복되는버프 들어가게해줘야함

        buffList.Add(buff);
    }

    public void ApplyUI()
    {
        switch (cardType)
        {
            case CardType.Potion:
                faceImage.sprite = ConstManager.Instance.potionSprite[value];
                break;
            case CardType.Sword:
                faceImage.sprite = ConstManager.Instance.swordSprite[value];
                break;
            case CardType.Sheild:
                faceImage.sprite = ConstManager.Instance.sheildSprite[value];
                break;
            case CardType.Monster:
                faceImage.sprite = ConstManager.Instance.monsterSprite[value];
                break;
            case CardType.Coin:
                faceImage.sprite = ConstManager.Instance.coinSprite[value];
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
