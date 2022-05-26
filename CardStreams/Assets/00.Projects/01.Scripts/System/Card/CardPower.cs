using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class CardPower : MonoBehaviour
{
    public Image faceImage;
    public Image backImage;
    public TextMeshProUGUI valueText;

    [Header("Debug")]
    public DropAreaType dropAreaType;
    public CardType cardType;
    public int value { get; private set; }
    public int goldP;   // 몬스터를 잡을 때 얻는 골드 배율, 기본 1
    public int originValue;

    // public List<BuffSO> buffList = new List<BuffSO>();
    public List<Buff> buffList = new List<Buff>();

    public void SetData_Feild(CardType cardType, int value)
    {
        this.dropAreaType = DropAreaType.feild;
        this.cardType = cardType;
        this.value = value;
        this.originValue = value;
        this.goldP = 1;

        ApplyUI();
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
        int tempValue = Mathf.Clamp(value, 0, ConstManager.Instance.potionSprite.Length - 1);


        switch (cardType)
        {
            case CardType.Potion:
                faceImage.sprite = ConstManager.Instance.potionSprite[tempValue];
                break;
            case CardType.Sword:
                faceImage.sprite = ConstManager.Instance.swordSprite[tempValue];
                break;
            case CardType.Sheild:
                faceImage.sprite = ConstManager.Instance.sheildSprite[tempValue];
                break;
            case CardType.Monster:
                faceImage.sprite = ConstManager.Instance.monsterSprite[tempValue];
                break;
            case CardType.Coin:
                faceImage.sprite = ConstManager.Instance.coinSprite[tempValue];
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
