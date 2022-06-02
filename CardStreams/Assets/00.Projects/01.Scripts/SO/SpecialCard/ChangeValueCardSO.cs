using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeValueCard", menuName = "ScriptableObject/SpecialCard/ChangeValueCard")]
public class ChangeValueCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int addAmount;

    public override void AccessBuildCard(Build build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        field.cardPower.AddValue(addAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
