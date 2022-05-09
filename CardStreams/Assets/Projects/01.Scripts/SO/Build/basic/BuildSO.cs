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

    // ����ؼ� ���⸦ ä���
    public virtual void AccessCard(Field field)
    {

    }

    // ����ؼ� ���⸦ ä���
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
