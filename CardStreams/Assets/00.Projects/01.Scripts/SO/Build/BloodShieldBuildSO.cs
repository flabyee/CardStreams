using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodShieldBuild", menuName = "ScriptableObject/Build/BloodShieldBuild")]
public class BloodShieldBuildSO : BuildSO
{
    [Header("amount")]
    public int hpAmount;
    public int shieldAmount;

    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        hpValue.RuntimeValue -= hpAmount;
        shieldValue.RuntimeValue += shieldAmount;

        playerValueChangeEvnet.Occurred();

        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
    }
}
