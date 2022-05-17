using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ApplyTiming
{
    NULL,
    Now,
    MoveStart,
    OnFeild,
    OnPlayer,
}

public abstract class SpecialCardSO : ScriptableObject
{
    public int id;
    public string specialCardName;
    public int price;
    [TextArea]
    public string tooltip;
    public Sprite sprite;

    public ApplyTiming applyTiming;
    public List<CardType> targetTypeList;

    public abstract void AccessSpecialCard(Player player, Field field);

    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvent;
}
