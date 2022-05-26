using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardData
{
    public CardData(CardType cardType, int value)
    {
        this.cardType = cardType;
        this.value = value;
    }
    public CardType cardType;
    public int value;
}
