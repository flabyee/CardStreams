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

    [Header("Amount")]
    public int addSwordAmount;

    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        swordValue.RuntimeValue += addSwordAmount;
        shieldValue.RuntimeValue = 0;

        playerValueChangeEvnet.Occurred();

        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
    }
}