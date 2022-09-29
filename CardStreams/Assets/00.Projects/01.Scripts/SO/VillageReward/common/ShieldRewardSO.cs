using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldReward", menuName = "SO/VillageReward/Shield")]
public class ShieldRewardSO : VillageRewardSO
{
    [Header("��������")]
    public IntValue shieldValue;
    public EventSO playerValueChangeEvent;

    [Header("��ġ")]
    public int increaseShield;

    public override void GetReward()
    {
        shieldValue.RuntimeValue += increaseShield;

        playerValueChangeEvent?.Occurred();
    }
}
