using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodShieldBuild", menuName = "ScriptableObject/Build/BloodShieldBuild")]
public class BloodTempleBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue hpValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvnet;

    [Header("amount")]
    public int hpAmount;
    public int shieldAmount;
    public int hpLimit;

    public override void AccessCard(Field field)
    {
    }

    public override void AccessPlayer(Player player)
    {
        if(hpValue.RuntimeValue > hpLimit)
        {
            hpValue.RuntimeValue -= hpAmount;
            shieldValue.RuntimeValue += shieldAmount;

            playerValueChangeEvnet.Occurred();

        }
    }
}
