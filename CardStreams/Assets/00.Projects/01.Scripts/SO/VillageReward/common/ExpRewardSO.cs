using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpReward", menuName = "SO/VillageReward/Exp")]
public class ExpRewardSO : VillageRewardSO
{
    [Header("수치")]
    public int getExpAmount;

    public override void GetReward()
    {
        GameManager.Instance.player.GetExp(getExpAmount); // 5개를 내보낸다 -> 실제 경험치 오르는건 1개만
    }
}