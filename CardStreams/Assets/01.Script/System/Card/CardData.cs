using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardData
{
    public CardData(CardType cardType, int value, DropAreaType dropAreaType)
    {
        this.cardType = cardType;
        this.value = value;
        this.dropAreaType = dropAreaType;
    }
    public CardType cardType;
    public int value;
    public DropAreaType dropAreaType;
}
