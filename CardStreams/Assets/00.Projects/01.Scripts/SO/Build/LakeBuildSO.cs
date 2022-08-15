using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lake", menuName = "ScriptableObject/Build/Lake")]
public class LakeBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue swordValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvnet;

    [Header("Amount")]
    public int fixValue;

    public override void AccessCard(Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        cardPower.SetValue(fixValue);
    }

    public override void AccessPlayer(Player player)
    {

    }
}
