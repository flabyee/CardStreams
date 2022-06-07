using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "CrossBuild", menuName = "ScriptableObject/Build/CrossBuild")]
public class CrossBuildSO : BuildSO
{
    [Header("Amount")]
    public int minusMonsterAmount;

    public override void AccessCard(Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        if (cardPower.basicType == BasicType.Monster)
        {
            cardPower.AddValue(-minusMonsterAmount);

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);

        }
    }

    public override void AccessPlayer(Player player)
    {
    }
}
