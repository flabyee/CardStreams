using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldReward", menuName = "SO/VillageReward/Gold")]
public class GoldRewardSO : VillageRewardSO
{
    [Header("설정변수")]
    public IntValue goldValue;
    public EventSO goldValueChangeEvent;

    [Header("수치")]
    public int getGoldAmount;

    public override void GetReward()
    {
        goldValue.RuntimeValue += getGoldAmount;

        goldValueChangeEvent?.Occurred();
    }
}
