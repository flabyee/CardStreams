using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemainSwordShieldPassive", menuName = "ScriptableObject/Passive/RemainSwordShield")]
public class RemainSwordShieldPassiveSO : PassiveSO
{
    [Header("��������")]
    public IntValue swordValue;
    public IntValue shieldValue;
    public EventSO playerValueChanged;

    [Header("��ġ")]
    public int addAmount;
    private int tileCount;

    public override void UseBuff(int fieldValue)
    {
        tileCount++;

        if (tileCount >= 4)
        {
            if(swordValue.RuntimeValue > 0)
            {
                swordValue.RuntimeValue += addAmount;
            }

            if (shieldValue.RuntimeValue > 0)
            {
                shieldValue.RuntimeValue += addAmount;
            }

            playerValueChanged?.Occurred();

            tileCount = 0;
        }
    }
}
