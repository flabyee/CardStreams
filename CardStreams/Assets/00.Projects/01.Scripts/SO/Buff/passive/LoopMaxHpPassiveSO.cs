using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopMaxHpPassive", menuName = "ScriptableObject/Passive/LoopMaxHp")]
public class LoopMaxHpPassiveSO : PassiveSO
{
    [Header("설정변수")]
    public IntValue hpValue;
    public EventSO playerValueChanged;

    [Header("수치")]
    public int addMaxHpAmount;
    private int tileCount;

    public override void UseBuff(int fieldValue)
    {
        tileCount++;

        if (tileCount >= 24)
        {
            // 기능
            hpValue.RuntimeMaxValue += addMaxHpAmount * currentLevel;
            playerValueChanged.Occurred();

            tileCount = 0;
        }
    }
}
