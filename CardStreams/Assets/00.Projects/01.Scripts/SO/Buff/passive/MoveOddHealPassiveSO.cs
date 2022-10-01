using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveOddHealPassive", menuName = "ScriptableObject/Passive/MoveOddHeal")]

public class MoveOddHealPassiveSO : PassiveSO
{
    [Header("��������")]
    public IntValue hpValue;
    public EventSO playerValueChanged;

    [Header("��ġ")]
    public int healAmount;
    private int tileCount;

    public override void UseBuff(int fieldValue)
    {
        tileCount++;

        if (tileCount >= 4)
        {
            // ���
            if (hpValue.RuntimeValue % 2 == 0) // ¦����
            {
                hpValue.RuntimeMaxValue += 1;
            }
            else // Ȧ����
            {
                hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount * currentLevel, 0, hpValue.RuntimeMaxValue);
            }

            playerValueChanged?.Occurred();

            tileCount = 0;
        }
    }
}
