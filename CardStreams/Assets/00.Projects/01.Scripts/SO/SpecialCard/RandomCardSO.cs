using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomCard", menuName = "ScriptableObject/SpecialCard/RandomCard")]
public class RandomCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.SetValue(Random.Range(1, 7));

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
