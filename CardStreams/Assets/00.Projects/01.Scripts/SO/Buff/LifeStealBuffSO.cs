using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStealBuffSO", menuName = "ScriptableObject/Buff/LifeStealBuffSO")]
public class LifeStealBuffSO : BuffSO
{
    public override int BuffOn(int totalDamage)
    {
        // 공격
        totalDamage -= swordValue.RuntimeValue;

        // 회복
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + swordValue.RuntimeValue, 0, hpValue.RuntimeMaxValue);

        // 검은 사라짐
        swordValue.RuntimeValue = 0;

        return totalDamage;
    }
}