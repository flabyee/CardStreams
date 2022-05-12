using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodShieldBuild", menuName = "ScriptableObject/Build/BloodShieldBuild")]
public class BloodShieldBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvnet;

    [Header("amount")]
    public int hpAmount;
    public int swordAmount;
    public int shieldAmount;

    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        hpValue.RuntimeValue -= hpAmount;
        swordValue.RuntimeValue += swordAmount;
        shieldValue.RuntimeValue += shieldAmount;

        playerValueChangeEvnet.Occurred();

        OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
    }
}
