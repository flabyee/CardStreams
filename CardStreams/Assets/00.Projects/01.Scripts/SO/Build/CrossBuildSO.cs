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
        if (field.cardPower.cardType == CardType.Monster)
        {
            field.cardPower.SetValue(field.cardPower.value - minusMonsterAmount);

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);

        }
    }

    public override void AccessPlayer(Player player)
    {
    }
}
