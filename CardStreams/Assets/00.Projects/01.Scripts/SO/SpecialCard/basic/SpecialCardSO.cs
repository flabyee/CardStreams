using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ApplyTiming
{
    NULL,
    NowField,
    MoveStart,
    OnFeild,
    ToPlayer,
}




public abstract class SpecialCardSO : ScriptableObject
{
    public int id;
    public string specialCardName;
    public int price;
    public CardGrade grade;
    [TextArea]
    public string tooltip;
    public Sprite sprite;

    public ApplyTiming applyTiming;
    public List<CardType> targetTypeList;
    public List<BasicType> targetBasicList;

    public abstract void AccessSpecialCard(Player player, Field field);
    public abstract void AccessBuildCard(BuildCard build);

}