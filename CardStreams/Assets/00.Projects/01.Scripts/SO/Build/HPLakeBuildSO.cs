using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HPLakeBuild", menuName = "ScriptableObject/Build/HPLakeBuild")]
public class HPLakeBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue hpValue;

    public EventSO playerValueChangeEvnet;

    [Header("Amount")]
    public int healAmount;
    public int maxUpAmount;
    public int hpLimit;

    public override void AccessCard(Field field)
    {

    }

    public override void AccessPlayer(Player player)
    {
        if (hpValue.RuntimeValue <= hpLimit)
        {
            if(hpValue.RuntimeValue + healAmount > hpValue.RuntimeMaxValue)
            {
                hpValue.RuntimeMaxValue += maxUpAmount;
            }
            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount, 0, hpValue.RuntimeMaxValue);

            playerValueChangeEvnet.Occurred();
        }
    }
}
