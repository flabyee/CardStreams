using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopMaxHpPassive", menuName = "ScriptableObject/Passive/LoopMaxHp")]
public class LoopMaxHpPassiveSO : PassiveSO
{
    [Header("��������")]
    public IntValue hpValue;
    public EventSO playerValueChanged;

    [Header("��ġ")]
    public int addMaxHpAmount;
    private int tileCount;

    public override void UseBuff(int fieldValue)
    {
        tileCount++;

        if (tileCount >= 24)
        {
            // ���
            hpValue.RuntimeMaxValue += addMaxHpAmount * currentLevel;
            playerValueChanged.Occurred();

            tileCount = 0;
        }
    }
}
