using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionToItemCard", menuName = "ScriptableObject/SpecialCard/PotionToItemCard")]
public class PotionToItemCardSO : SpecialCardSO
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
        cardPower.basicType = Random.Range(0, 2) == 0 ? BasicType.Sword : BasicType.Sheild;
        cardPower.AddValue(upAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
