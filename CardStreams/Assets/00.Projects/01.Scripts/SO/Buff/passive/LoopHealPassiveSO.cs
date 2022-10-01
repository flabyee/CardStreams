using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopHealPassive", menuName = "ScriptableObject/Passive/LoopHeal")]

public class LoopHealPassiveSO : PassiveSO
{
    [Header("설정변수")]
    public IntValue hpValue;
    public EventSO playerValueChanged;

    [Header("수치")]
    public int healAmount;
    private int tileCount;

    public override void UseBuff(int fieldValue)
    {
        tileCount++;

        if (tileCount >= 24)
        {
            // 기능
            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount * currentLevel, 0, hpValue.RuntimeMaxValue);
            playerValueChanged.Occurred();

            tileCount = 0;
        }
    }
}
