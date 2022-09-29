using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldReward", menuName = "SO/VillageReward/Shield")]
public class ShieldRewardSO : VillageRewardSO
{
    [Header("설정변수")]
    public IntValue shieldValue;
    public EventSO playerValueChangeEvent;

    [Header("수치")]
    public int increaseShield;

    public override void GetReward()
    {
        shieldValue.RuntimeValue += increaseShield;

        playerValueChangeEvent?.Occurred();
    }
}
