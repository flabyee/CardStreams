using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SacrificeCardSO", menuName = "ScriptableObject/SpecialCard/SacrificeCardSO")]
public class SacrificeCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int limitDamage;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        // cardPower
        int sacrificeDamage = Mathf.Clamp(player.hpValue.RuntimeMaxValue - player.hpValue.RuntimeValue, 0, limitDamage);

        cardPower.AddValue(-sacrificeDamage);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
