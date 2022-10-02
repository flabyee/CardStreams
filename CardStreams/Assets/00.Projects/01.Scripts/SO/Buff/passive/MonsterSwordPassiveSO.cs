using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSwordPassive", menuName = "ScriptableObject/Passive/MonsterSword")]
public class MonsterSwordPassiveSO : PassiveSO
{
    [Header("설정변수")]
    public IntValue swordValue;
    public EventSO playerValueChanged;

    [Header("수치")]
    public int addSwordAmount;

    public override void UseBuff(int fieldValue)
    {
        if(fieldValue > swordValue.RuntimeValue) // 몬스터카드가 칼보다 쎄다면
        {
            swordValue.RuntimeValue += addSwordAmount * currentLevel;
            playerValueChanged.Occurred();
        }
    }
}