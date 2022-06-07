using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BleedSwordCard", menuName = "ScriptableObject/SpecialCard/BleedSwordCard")]
public class BleedSwordCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int upSwordAmount;
    public int lessHpAmount;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        // player
        player.hpValue.RuntimeValue -= lessHpAmount;
        player.playerValueChangeEvent.Occurred();

        // cardPower
        cardPower.AddValue(upSwordAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    } 
}