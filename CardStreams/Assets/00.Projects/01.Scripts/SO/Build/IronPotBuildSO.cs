using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IronPotBuild", menuName = "ScriptableObject/Build/IronPotBuild")]
public class IronPotBuildSO : BuildSO
{
    [Header("Amount")]
    public int amount;

    public override void AccessCard(Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        if(cardPower.basicType == BasicType.Monster)
        {
            cardPower.basicType = BasicType.Potion;
            cardPower.SetValue(Mathf.RoundToInt(cardPower.value / amount));
        }
    }

    public override void AccessPlayer(Player player)
    {
        
    }
}
