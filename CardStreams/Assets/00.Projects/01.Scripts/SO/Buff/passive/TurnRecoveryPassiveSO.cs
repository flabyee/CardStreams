using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnRecoveryPassive", menuName = "ScriptableObject/Passive/TurnRecovery")]

public class TurnRecoveryPassiveSO : PassiveSO
{
    public IntValue hpValue;
    public EventSO playerValueChanged;

    private int tileCount;

    public override void UseBuff(int fieldValue)
    {
        tileCount++;

        if(tileCount >= 4)
        {
            hpValue.RuntimeValue += currentLevel;
            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + currentLevel, 0, hpValue.RuntimeMaxValue);
            playerValueChanged.Occurred();
            tileCount = 0;
        }
    }
}