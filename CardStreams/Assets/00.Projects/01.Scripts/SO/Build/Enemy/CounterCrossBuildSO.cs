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
        BasicCard cardPower = field.cardPower as BasicCard;

        if (cardPower.basicType == BasicType.Monster)
        {
            cardPower.SetValue(cardPower.value + addMonsterAmount);

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
        }
    }
}
