using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterAttackCard", menuName = "ScriptableObject/SpecialCard/MonsterAttackCard")]
public class MonsterAttackCardSO : SpecialCardSO
{
    [Header("amounts")]
    int attackAmount;
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.AddValue(-attackAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}