using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStealBuffSO", menuName = "ScriptableObject/Buff/LifeStealBuffSO")]
public class LifeStealBuffSO : BuffSO
{
    public IntValue hpValue;
    public IntValue swordValue;



    public override void UseBuff(int fieldValue)
    {
        // 칼 공격력만큼 회복
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + swordValue.RuntimeValue, 0, hpValue.RuntimeMaxValue);
    }
}