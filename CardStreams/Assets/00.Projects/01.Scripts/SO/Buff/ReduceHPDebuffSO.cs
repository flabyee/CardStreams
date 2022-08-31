using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReduceHPDebuffSO", menuName = "ScriptableObject/Buff/ReduceHPDebuffSO")]
public class ReduceHPDebuffSO : BuffSO
{
    public IntValue hpValue;
    public IntValue swordValue;



    public override void UseBuff(int fieldValue)
    {
        Debug.Log(fieldValue);
        // 칼 공격력 / 10만큼 데미지입음, 나머지는 올림처리
        int damage = fieldValue / 5;
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue - damage, 1, hpValue.RuntimeMaxValue);
    }
}
