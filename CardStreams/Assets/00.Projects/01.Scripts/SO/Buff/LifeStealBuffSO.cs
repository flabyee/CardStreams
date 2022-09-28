using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStealBuffSO", menuName = "ScriptableObject/Buff/LifeStealBuffSO")]
public class LifeStealBuffSO : BuffSO
{
    public int limitAmount;

    public IntValue hpValue;
    public IntValue swordValue;



    public override void UseBuff(int fieldValue)
    {
        // 칼 공격력만큼 회복
        int healAmount = Mathf.Clamp(swordValue.RuntimeValue, 0, limitAmount);

        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount, 0, hpValue.RuntimeMaxValue);
    }
}