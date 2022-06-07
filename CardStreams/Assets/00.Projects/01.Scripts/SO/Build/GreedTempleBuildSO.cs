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
        BasicCard cardPower = field.cardPower as BasicCard;

        if (cardPower.basicType == BasicType.Monster)
        {
            cardPower.goldP = goldPAmount;

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
        }
    }
}
