using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BleedSwordCard", menuName = "ScriptableObject/SpecialCard/BleedSwordCard")]
public class BleedSwordCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // player
        hpValue.RuntimeValue -= 2;
        playerValueChangeEvent.Occurred();

        // cardPower
        field.cardPower.value += 5;
        field.cardPower.ApplyUI();

        // apply filed
        field.SetData(field.cardPower);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    } 
}