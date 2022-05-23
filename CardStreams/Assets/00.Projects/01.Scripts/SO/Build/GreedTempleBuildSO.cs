using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GreedTempleBuild", menuName = "ScriptableObject/Build/GreedTempleBuild")]
public class GreedTempleBuildSO : BuildSO
{
    [Header("amount")]
    public int goldPAmount;

    public override void AccessCard(Field field)
    {
        if (field.cardPower.cardType == CardType.Monster)
        {
            field.cardPower.goldP = goldPAmount;

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
        }
    }
}
