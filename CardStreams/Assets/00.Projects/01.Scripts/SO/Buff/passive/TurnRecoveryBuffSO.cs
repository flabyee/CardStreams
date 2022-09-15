using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnRecoveryBuff", menuName = "ScriptableObject/Buff/TurnRecoveryBuff")]

public class TurnRecoveryBuffSO : PassiveSO
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
            playerValueChanged.Occurred();
            tileCount = 0;
        }
    }
}
