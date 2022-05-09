using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodShieldBuild", menuName = "ScriptableObject/Build/BloodShieldBuild")]
public class BloodShieldBuildSO : BuildSO
{
    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        hpValue.RuntimeValue -= 2;
        shieldValue.RuntimeValue += 3;

        playerValueChangeEvnet.Occurred();

        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
    }
}
