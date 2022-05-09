using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SacrificeCardSO", menuName = "ScriptableObject/SpecialCard/SacrificeCardSO")]
public class SacrificeCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        int sacrificeDamage = hpValue.RuntimeMaxValue - hpValue.RuntimeValue;

        field.cardPower.SetValue(field.cardPower.value - sacrificeDamage);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
