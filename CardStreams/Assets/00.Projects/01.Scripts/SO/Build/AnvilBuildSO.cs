using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnvilBuild", menuName = "ScriptableObject/Build/AnvilBuild")]
public class AnvilBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue swordValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvnet;

    [Header("Amount")]
    public int dontHaveAmount;
    public int haveAmount;

    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        if (swordValue.RuntimeValue == 0)
        {
            swordValue.RuntimeValue = dontHaveAmount;
        }
        else
        {
            swordValue.RuntimeValue += haveAmount;
        }
        if (shieldValue.RuntimeValue == 0)
        {
            shieldValue.RuntimeValue = dontHaveAmount;
        }
        else
        {
            shieldValue.RuntimeValue += haveAmount;
        }

        playerValueChangeEvnet.Occurred();

        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
    }
}
