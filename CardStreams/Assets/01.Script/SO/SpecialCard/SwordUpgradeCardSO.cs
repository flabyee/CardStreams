using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordUpgradeCard", menuName = "ScriptableObject/SpecialCard/SwordUpgradeCard")]
public class SwordUpgradeCardSO : SpecialCardSO
{
    public override void AccessSpecialCard(Player player, Field field)
    {
        field.cardPower.value += 2;

        field.cardPower.ApplyUI();

        field.SetData(field.cardPower);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
