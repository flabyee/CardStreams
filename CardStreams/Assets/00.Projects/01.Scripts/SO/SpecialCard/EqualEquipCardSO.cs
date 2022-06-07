using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EqualEquip", menuName = "ScriptableObject/SpecialCard/EqualEquip")]
public class EqualEquipCardSO : SpecialCardSO
{
    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        int sum = Mathf.RoundToInt(player.swordValue.RuntimeValue + player.shieldValue.RuntimeValue); // 반올림
        if (sum != 0) // 0이면 에러남 DivisionByZero
        {
            player.swordValue.RuntimeValue = sum / 2;
            player.shieldValue.RuntimeValue = sum / 2;
        }

        // 플레이어한테 쓰는거라 field 아닌 player 위치
        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
        player.playerValueChangeEvent.Occurred();
    }
}