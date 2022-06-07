using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemToPotionCard", menuName = "ScriptableObject/SpecialCard/ItemToPotionCard")]
public class ItemToPotionCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int upAmount;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        // cardPower
        cardPower.basicType = BasicType.Potion;

        cardPower.AddValue(upAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
