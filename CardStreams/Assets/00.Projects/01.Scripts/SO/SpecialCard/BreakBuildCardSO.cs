using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreakBuildCard", menuName = "ScriptableObject/SpecialCard/BreakBuildCard")]
public class BreakBuildCardSO : SpecialCardSO
{
    public override void AccessBuildCard(Build build)
    {
        build.BuildUp();

        build.gameObject.SetActive(false);

        OnFieldTooltip.Instance.ShowBuild(build.transform.position, sprite);
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        
    }
}