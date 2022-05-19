using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionUpBuild", menuName = "ScriptableObject/Build/PotionUpBuild")]
public class PotionUpBuildSO : BuildSO
{
    [Header("Amount")]
    public int plusPotionAmount;

    public override void AccessCard(Field field)
    {
        if (field.cardPower.cardType == CardType.Potion)
        {
            field.cardPower.SetValue(field.cardPower.value - plusPotionAmount);

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
        }
    }

    public override void AccessPlayer(Player player)
    {
    }
}
