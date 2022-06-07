using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EqualAttack", menuName = "ScriptableObject/SpecialCard/EqualAttack")]
public class EqualAttackCardSO : SpecialCardSO
{
    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        if(player.hpValue.RuntimeValue == player.shieldValue.RuntimeValue)
        {
            cardPower.SetValue(0);
        }

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}

