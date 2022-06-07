using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomCard", menuName = "ScriptableObject/SpecialCard/RandomCard")]
public class RandomCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int mulAmount;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        // cardPower
        cardPower.SetValue(Random.Range(1, cardPower.value * mulAmount));

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
