using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowShield", menuName = "ScriptableObject/SpecialCard/ThrowShield")]
public class ThrowShieldCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.AddValue(-player.shieldValue.RuntimeValue);

        player.shieldValue.RuntimeValue = 0;

        player.playerValueChangeEvent.Occurred();

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
