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
        field.cardPower.value = (Mathf.RoundToInt(field.value / 3f)) * 2;

        field.cardPower.ApplyUI();

        // apply field
        field.SetData(field.cardPower);

        Debug.Log("potion to item");

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
