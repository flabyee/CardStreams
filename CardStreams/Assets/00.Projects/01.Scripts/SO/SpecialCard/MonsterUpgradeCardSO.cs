using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterUpgrade", menuName = "ScriptableObject/SpecialCard/MonsterUpgrade")]
public class MonsterUpgradeCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower
        field.cardPower.SetValue(Mathf.RoundToInt(field.cardPower.value * 1.5f));
        field.cardPower.goldP = 3;
        

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
