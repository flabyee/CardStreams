using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnvilBuild", menuName = "ScriptableObject/Build/AnvilBuild")]
public class AnvilBuildSO : BuildSO
{
    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        if (swordValue.RuntimeValue == 0)
        {
            swordValue.RuntimeValue = 2;
        }
        else
        {
            swordValue.RuntimeValue += 1;
        }
        if (shieldValue.RuntimeValue == 0)
        {
            shieldValue.RuntimeValue = 2;
        }
        else
        {
            shieldValue.RuntimeValue += 1;
        }

        playerValueChangeEvnet.Occurred();

        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
    }
}
