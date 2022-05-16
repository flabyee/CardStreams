using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionToItemCard", menuName = "ScriptableObject/SpecialCard/PotionToItemCard")]
public class PotionToItemCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.cardType = Random.Range(0, 2) == 0 ? CardType.Sword : CardType.Sheild;
        field.cardPower.SetValue(Random.Range(0, field.cardPower.value * 2));

        Debug.Log("potion to item");

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
