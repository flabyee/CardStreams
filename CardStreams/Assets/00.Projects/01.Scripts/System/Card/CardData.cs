using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardType
{
    NULL,
    Sword,
    Sheild,
    Potion,
    Monster,
    Coin,
    Special,
    Build,
}

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
