using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemToPotionCard", menuName = "ScriptableObject/SpecialCard/ItemToPotionCard")]
public class ItemToPotionCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.cardType = CardType.Potion;
        field.cardPower.SetValue(Random.Range(0, field.cardPower.value * 2));
        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
