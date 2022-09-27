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
        //playerValueChangeEvent.Occured(); 이거는 GetReward 부르는곳에서 딱1번
    }
}
