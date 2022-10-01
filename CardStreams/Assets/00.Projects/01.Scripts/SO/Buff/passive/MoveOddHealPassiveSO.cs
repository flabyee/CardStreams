using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveOddHealPassive", menuName = "ScriptableObject/Passive/MoveOddHeal")]

public class MoveOddHealPassiveSO : PassiveSO
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

        if (tileCount >= 4)
        {
            // 기능
            if (hpValue.RuntimeValue % 2 == 0) // 짝수다
            {
                hpValue.RuntimeMaxValue += 1;
            }
            else // 홀수다
            {
                hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount * currentLevel, 0, hpValue.RuntimeMaxValue);
            }

            playerValueChanged?.Occurred();

            tileCount = 0;
        }
    }
}
