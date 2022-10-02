using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSwordPassive", menuName = "ScriptableObject/Passive/MonsterSword")]
public class MonsterSwordPassiveSO : PassiveSO
{
    [Header("��������")]
    public IntValue swordValue;
    public EventSO playerValueChanged;

    [Header("��ġ")]
    public int addSwordAmount;

    public override void UseBuff(int fieldValue)
    {
        if(fieldValue > swordValue.RuntimeValue) // ����ī�尡 Į���� ��ٸ�
        {
            swordValue.RuntimeValue += addSwordAmount * currentLevel;
            playerValueChanged.Occurred();
        }
    }
}