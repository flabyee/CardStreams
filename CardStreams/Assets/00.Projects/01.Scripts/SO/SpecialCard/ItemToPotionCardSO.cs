using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemToPotionCard", menuName = "ScriptableObject/SpecialCard/ItemToPotionCard")]
public class ItemToPotionCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int upAmount;

    public override void AccessBuildCard(Build build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.cardType = CardType.Potion;

        field.cardPower.AddValue(upAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
