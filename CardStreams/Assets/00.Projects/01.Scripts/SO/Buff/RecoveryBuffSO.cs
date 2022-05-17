using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryBuffSO", menuName = "ScriptableObject/Buff/RecoveryBuffSO")]
public class RecoveryBuffSO : BuffSO
{
    public IntValue hpValue;

    [Header("회복량")]
    public int recoveryAmount = 2;



    public override void UseBuff(int fieldValue)
    {
        // 2만큼 회복
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + recoveryAmount, 0, hpValue.RuntimeMaxValue);
    }
}