using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnRecoveryBuf", menuName = "ScriptableObject/Buff/TurnRecoveryBuf")]

public class TurnRecoveryBuffSO : BuffSO
{
    public IntValue hpValue;
    public EventSO playerValueChanged;

    private int tileCount;

    public override void UseBuff(int fieldValue)
    {
        Debug.Log("4ĭ��");
        tileCount++;

        if(tileCount >= 4)
        {
            Debug.Log("��");
            hpValue.RuntimeValue += 1;
            playerValueChanged.Occurred();
            tileCount = 0;
        }

        
    }
}
