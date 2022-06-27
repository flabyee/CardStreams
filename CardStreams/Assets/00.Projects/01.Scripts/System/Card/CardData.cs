using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum CardType
{
    NULL,
    Basic,
    Special,
    Build,
}

[Serializable]
public class CardData
{
    public CardData(BasicType basicType, int value)
    {
        this.basicType = basicType;
        this.value = value;
    }
    public BasicType basicType;
    public int value;
}
