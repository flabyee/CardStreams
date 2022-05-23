using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreakBuildCard", menuName = "ScriptableObject/SpecialCard/BreakBuildCard")]
public class BreakBuildCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}