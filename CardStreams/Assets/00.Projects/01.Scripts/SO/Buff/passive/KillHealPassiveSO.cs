using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillHealPassive", menuName = "ScriptableObject/Passive/KillHeal")]
public class KillHealPassiveSO : PassiveSO
{
    [Header("설정변수")]
    public IntValue hpValue;
    public EventSO playerValueChanged;

    [Header("수치")]
    public int healAmount;

    public override void UseBuff(int fieldValue)
    {
        hpValue.RuntimeValue += healAmount * currentLevel;
        playerValueChanged.Occurred();
    }
}
