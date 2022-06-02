using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterUpgrade", menuName = "ScriptableObject/SpecialCard/MonsterUpgrade")]
public class GoldMonsterCardSO : SpecialCardSO
{
    [Header("amounts")]
    public float mulAmount;
    public int goldPAmount;

    public override void AccessBuildCard(Build build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.SetValue(Mathf.RoundToInt(field.cardPower.value * mulAmount));
        field.cardPower.goldP = goldPAmount;
        

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
