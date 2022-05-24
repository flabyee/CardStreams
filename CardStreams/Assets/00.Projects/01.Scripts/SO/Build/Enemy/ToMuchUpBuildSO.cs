using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToMuchUpBuild", menuName = "ScriptableObject/Build/Enemy/ToMuchUpBuild")]
public class ToMuchUpBuildSO : EnemyBuildSO
{
    [Header("Amount")]
    public int upAmount;

    public override void AccessCard(Field field)
    {
        field.cardPower.AddValue(upAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
