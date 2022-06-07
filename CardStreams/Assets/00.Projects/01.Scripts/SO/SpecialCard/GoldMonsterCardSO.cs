using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterUpgrade", menuName = "ScriptableObject/SpecialCard/MonsterUpgrade")]
public class GoldMonsterCardSO : SpecialCardSO
{
    [Header("amounts")]
    public float mulAmount;
    public int goldPAmount;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        // cardPower
        cardPower.SetValue(Mathf.RoundToInt(cardPower.value * mulAmount));
        cardPower.goldP = goldPAmount;
        

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
