using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleShieldPassive", menuName = "ScriptableObject/Passive/BattleShield")]
public class BattleShieldPassiveSO : PassiveSO
{
    [Header("설정변수")]
    public IntValue shieldValue;
    public EventSO playerValueChanged;

    private int tileCount;
    private int monsterCount;

    public override void Init(Passive passive)
    {
        base.Init(passive);

        MissionObserverManager.instance.OnBasicCard += OnMonsterCard;
    }

    public override void UseBuff(int fieldValue)
    {
        tileCount++;

        if (tileCount >= 4)
        {
            shieldValue.RuntimeValue += monsterCount * currentLevel;

            playerValueChanged?.Occurred();

            monsterCount = 0;
            tileCount = 0;
        }
    }

    public void OnMonsterCard(BasicCard card)
    {
        if(card.basicType == BasicType.Monster)
        {
            monsterCount++;
        }
    }
}
