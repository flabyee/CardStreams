using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverhealBuffSO", menuName = "ScriptableObject/Buff/OverhealBuffSO")]
public class OverHealBuffSO : BuffSO
{
    public IntValue hpValue;

    [Header("초과 회복량")]
    public int overHeal = 0;



    public override void UseBuff(int fieldValue)
    {
        int overHealAmount = fieldValue - (hpValue.RuntimeMaxValue - hpValue.RuntimeValue); // 오버힐량 = 포션회복수치 - (최대체력 - 현재체력)
        Debug.Log(overHealAmount);

        if (overHealAmount > 0) // 오버힐 발동시
        {
            
            hpValue.RuntimeMaxValue += overHealAmount;
        }
    }
}