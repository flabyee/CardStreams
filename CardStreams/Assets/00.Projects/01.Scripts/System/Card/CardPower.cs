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

    [Header("Debug")]
    public CardType cardType;

    public virtual void SetData_Feild(BasicType basicType, int value)
    {
        this.cardType = CardType.Basic;
    }

    public void SetData_SpecialCard()
    {
        this.cardType = CardType.Special;
    }

    public void SetData_Build()
    {
        this.cardType = CardType.Build;
    }

    
}
