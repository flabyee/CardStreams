using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SacrificeCardSO", menuName = "ScriptableObject/SpecialCard/SacrificeCardSO")]
public class SacrificeCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.value = Mathf.Clamp(field.cardPower.value - Mathf.Abs(hpValue.RuntimeValue - hpValue.RuntimeMaxValue), 0, 99);

        field.cardPower.ApplyUI();

        // field Apply
        field.SetData(field.cardPower);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
