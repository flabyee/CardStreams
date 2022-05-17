using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverhealBuffSO", menuName = "ScriptableObject/Buff/OverhealBuffSO")]
public class OverHealBuffSO : BuffSO
{
    public IntValue hpValue;

    [Header("�ʰ� ȸ����")]
    public int overHeal = 0;



    public override void UseBuff(int fieldValue)
    {
        int overHealAmount = fieldValue - (hpValue.RuntimeMaxValue - hpValue.RuntimeValue); // �������� = ����ȸ����ġ - (�ִ�ü�� - ����ü��)
        Debug.Log(overHealAmount);

        if (overHealAmount > 0) // ������ �ߵ���
        {
            
            hpValue.RuntimeMaxValue += overHealAmount;
        }
    }
}