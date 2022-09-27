using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxHpRewardSO", menuName = "SO/VillageReward/MaxHP")]
public class MaxHpRewardSO : VillageRewardSO
{
    public IntValue hpValue; 

    public override void GetReward()
    {
        hpValue.RuntimeMaxValue += 2;
        hpValue.RuntimeValue += 2;
        //playerValueChangeEvent.Occured(); �̰Ŵ� GetReward �θ��°����� ��1��
    }
}
