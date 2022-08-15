using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mountains", menuName = "ScriptableObject/Build/Mountains")]
public class MountainsBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue hpValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvnet;

    public override void AccessCard(Field field)
    {

    }

    public override void AccessPlayer(Player player)
    {
        hpValue.RuntimeMaxValue += shieldValue.RuntimeValue;
        shieldValue.RuntimeValue = 0;

        playerValueChangeEvnet.Occurred();
    }
}
