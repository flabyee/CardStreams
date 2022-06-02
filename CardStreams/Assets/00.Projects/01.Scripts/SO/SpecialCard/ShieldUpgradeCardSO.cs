using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldUpgradeCard", menuName = "ScriptableObject/SpecialCard/ShieldUpgradeCard")]
public class ShieldUpgradeCardSO : SpecialCardSO
{
    public override void AccessBuildCard(Build build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        field.cardPower.AddValue(2);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
