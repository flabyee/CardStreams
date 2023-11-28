using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillHealPassive", menuName = "ScriptableObject/Passive/KillHeal")]
public class KillHealPassiveSO : PassiveSO
{
    [Header("��������")]
    public IntValue hpValue;
    public EventSO playerValueChanged;

    [Header("��ġ")]
    public int healAmount;

    public override void UseBuff(int fieldValue)
    {
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount * currentLevel, 0, hpValue.RuntimeMaxValue);
        playerValueChanged.Occurred();
    }
}