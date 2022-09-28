using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxHealthUpBuild", menuName = "ScriptableObject/Build/MaxHealthUpBuild")]
public class MaxHealthUpBuildSO : BuildSO
{
    public int upAmount;

    [Header("SO")]
    public IntValue hpValue;


    public EventSO playerValueChangeEvnet;

    public override void AccessCard(Field field)
    {

    }

    public override void AccessPlayer(Player player)
    {
        hpValue.RuntimeMaxValue += upAmount;

        playerValueChangeEvnet.Occurred();
    }
}
