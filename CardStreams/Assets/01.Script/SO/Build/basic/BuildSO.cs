using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildSO : ScriptableObject
{
    public int id;
    public string buildName;
    public int price;
    [TextArea]
    public string tooltip;
    public Sprite sprite;

    public List<int> areaList;

    // 상속해서 여기를 채운다
    public virtual void AccessCard(Field field)
    {

    }

    // 상속해서 여기를 채운다
    public virtual void AccessPlayer(Player player)
    {

    }

    public virtual void AccessTurnEnd()
    {
        
    }

    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;

    public IntValue goldValue;

    public EventSO playerValueChangeEvnet;
    public EventSO goldChangeEvnet;
}
