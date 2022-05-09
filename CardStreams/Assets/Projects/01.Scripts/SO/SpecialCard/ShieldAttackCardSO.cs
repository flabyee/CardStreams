using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldAttackCardSO", menuName = "ScriptableObject/SpecialCard/ShieldAttackCardSO")]
public class ShieldAttackCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.value = field.cardPower.value -= shieldValue.RuntimeValue;

        field.cardPower.ApplyUI();

        // field Apply
        field.SetData(field.cardPower);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
