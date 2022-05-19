using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CounterCrossBuild", menuName = "ScriptableObject/Build/Enemy/CounterCrossBuild")]
public class CounterCrossBuildSO : EnemyBuildSO
{
    [Header("Amount")]
    public int addMonsterAmount;

    public override void AccessCard(Field field)
    {
        if (field.cardPower.cardType == CardType.Monster)
        {
            field.cardPower.SetValue(field.cardPower.value + addMonsterAmount);

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
        }
    }
}
