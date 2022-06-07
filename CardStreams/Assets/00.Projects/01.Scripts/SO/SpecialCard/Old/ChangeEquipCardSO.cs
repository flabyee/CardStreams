using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeEquip", menuName = "ScriptableObject/SpecialCard/ChangeEquip")]
public class ChangeEquipCardSO : SpecialCardSO
{
    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        int temp = player.swordValue.RuntimeValue; // 값 교환
        player.swordValue.RuntimeValue = player.shieldValue.RuntimeValue;
        player.shieldValue.RuntimeValue = temp;

        if(player.swordValue.RuntimeValue == player.shieldValue.RuntimeValue) // 같으면 둘다 +2
        {
            player.swordValue.RuntimeValue += 2;
            player.shieldValue.RuntimeValue += 2;
        }

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
        player.playerValueChangeEvent.Occurred();
    }
}