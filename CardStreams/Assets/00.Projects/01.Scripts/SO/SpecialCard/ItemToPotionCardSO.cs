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
        field.cardPower.SetValue(Mathf.RoundToInt(field.cardPower.value / 3f) * 2);
        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
