using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BleedSwordCard", menuName = "ScriptableObject/SpecialCard/BleedSwordCard")]
public class BleedSwordCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int upSwordAmount;
    public int lessHpAmount;

    public override void AccessBuildCard(Build build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        // player
        player.hpValue.RuntimeValue -= lessHpAmount;
        player.playerValueChangeEvent.Occurred();

        // cardPower
        field.cardPower.AddValue(upSwordAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    } 
}