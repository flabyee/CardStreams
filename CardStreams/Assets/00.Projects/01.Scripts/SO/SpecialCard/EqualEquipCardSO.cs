using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EqualEquip", menuName = "ScriptableObject/SpecialCard/EqualEquip")]
public class EqualEquipCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        int sum = Mathf.RoundToInt(swordValue.RuntimeValue + shieldValue.RuntimeValue); // 반올림
        if (sum != 0) // 0이면 에러남 DivisionByZero
        {
            swordValue.RuntimeValue = sum / 2;
            shieldValue.RuntimeValue = sum / 2;
        }

        // 플레이어한테 쓰는거라 field 아닌 player 위치
        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
        playerValueChangeEvent.Occurred();
    }
}