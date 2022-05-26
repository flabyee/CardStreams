using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionToItemCard", menuName = "ScriptableObject/SpecialCard/PotionToItemCard")]
public class PotionToItemCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int upAmount;

    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.cardType = Random.Range(0, 2) == 0 ? CardType.Sword : CardType.Sheild;
        field.cardPower.AddValue(upAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
