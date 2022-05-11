using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStealBuffSO", menuName = "ScriptableObject/Buff/LifeStealBuffSO")]
public class LifeStealBuffSO : BuffSO
{
    public override int BuffOn(int totalDamage)
    {
        // ����
        totalDamage -= swordValue.RuntimeValue;

        // ȸ��
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + swordValue.RuntimeValue, 0, hpValue.RuntimeMaxValue);

        // ���� �����
        swordValue.RuntimeValue = 0;

        return totalDamage;
    }
}