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
        int sum = Mathf.RoundToInt(player.swordValue.RuntimeValue + player.shieldValue.RuntimeValue); // �ݿø�
        if (sum != 0) // 0�̸� ������ DivisionByZero
        {
            player.swordValue.RuntimeValue = sum / 2;
            player.shieldValue.RuntimeValue = sum / 2;
        }

        // �÷��̾����� ���°Ŷ� field �ƴ� player ��ġ
        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
        player.playerValueChangeEvent.Occurred();
    }
}