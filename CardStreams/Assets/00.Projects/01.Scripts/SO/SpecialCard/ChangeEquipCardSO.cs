using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeEquip", menuName = "ScriptableObject/SpecialCard/ChangeEquip")]
public class ChangeEquipCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        int temp = swordValue.RuntimeValue; // �� ��ȯ
        swordValue.RuntimeValue = shieldValue.RuntimeValue;
        shieldValue.RuntimeValue = temp;

        if(swordValue.RuntimeValue == shieldValue.RuntimeValue) // ������ �Ѵ� +2
        {
            swordValue.RuntimeValue += 2;
            shieldValue.RuntimeValue += 2;
        }

        

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}