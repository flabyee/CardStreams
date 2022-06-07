using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordUpgradeCard", menuName = "ScriptableObject/SpecialCard/SwordUpgradeCard")]
public class SwordUpgradeCardSO : SpecialCardSO
{
    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        cardPower.AddValue(2);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
