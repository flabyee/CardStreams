using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EqualAttack", menuName = "ScriptableObject/SpecialCard/EqualAttack")]
public class EqualAttackCardSO : SpecialCardSO
{

    public override void AccessSpecialCard(Player player, Field field)
    {
        if(player.hpValue.RuntimeValue == player.shieldValue.RuntimeValue)
        {
            field.cardPower.SetValue(0);
        }

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}

