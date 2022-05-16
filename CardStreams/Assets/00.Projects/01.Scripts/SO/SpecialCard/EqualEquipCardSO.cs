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

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}