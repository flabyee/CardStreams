using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxHpReward", menuName = "SO/VillageReward/MaxHP")]
public class MaxHpRewardSO : VillageRewardSO
{
    [Header("설정변수")]
    public IntValue hpValue;
    public EventSO playerValueChangeEvent;

    [Header("수치")]
    public int increaseHp;

    public override void GetReward()
    {
        hpValue.RuntimeMaxValue += increaseHp;
        hpValue.RuntimeValue += increaseHp;

        playerValueChangeEvent.Occurred();
    }
}
