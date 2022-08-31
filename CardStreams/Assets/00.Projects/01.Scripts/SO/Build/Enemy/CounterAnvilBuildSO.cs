using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CounterAnvilBuild", menuName = "ScriptableObject/Build/Enemy/CounterAnvilBuild")]
public class CounterAnvilBuildSO : EnemyBuildSO
{
    [Header("SO")]
    public IntValue swordValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvnet;


    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        swordValue.RuntimeValue = 0;

        playerValueChangeEvnet.Occurred();

    }
}