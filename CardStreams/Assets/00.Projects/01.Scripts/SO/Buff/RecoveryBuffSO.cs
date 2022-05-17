using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryBuffSO", menuName = "ScriptableObject/Buff/RecoveryBuffSO")]
public class RecoveryBuffSO : BuffSO
{
    public IntValue hpValue;

    [Header("ȸ����")]
    public int recoveryAmount = 2;



    public override void UseBuff(int fieldValue)
    {
        // 2��ŭ ȸ��
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + recoveryAmount, 0, hpValue.RuntimeMaxValue);
    }
}