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
        if(field.cardPower.cardType == CardType.Monster)
        {
            field.cardPower.cardType = CardType.Potion;
            field.cardPower.SetValue(Mathf.RoundToInt(field.cardPower.value / amount));
        }
    }

    public override void AccessPlayer(Player player)
    {
        
    }
}
