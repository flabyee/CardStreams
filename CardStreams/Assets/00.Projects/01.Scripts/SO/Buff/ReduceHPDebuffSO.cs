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
        // Į ���ݷ� / 10��ŭ ����������, �������� �ø�ó��
        int damage = fieldValue / 5;
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue - damage, 1, hpValue.RuntimeMaxValue);
    }
}
