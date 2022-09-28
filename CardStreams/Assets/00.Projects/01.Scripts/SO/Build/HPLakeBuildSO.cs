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
    public float healAmountPer;
    public float hpLimitPer;    // 0~1
    public int limitAmount;

    public override void AccessCard(Field field)
    {

    }

    public override void AccessPlayer(Player player)
    {
        int healAmount = Mathf.Clamp(Mathf.RoundToInt(hpValue.RuntimeMaxValue * healAmountPer), 0, limitAmount);

        if (hpValue.RuntimeValue <= hpValue.RuntimeMaxValue * hpLimitPer)
        {
            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount, 0, hpValue.RuntimeMaxValue);

            playerValueChangeEvnet.Occurred();
        }
    }
}
