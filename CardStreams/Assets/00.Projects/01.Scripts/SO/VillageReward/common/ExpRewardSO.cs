using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpReward", menuName = "SO/VillageReward/Exp")]
public class ExpRewardSO : VillageRewardSO
{
    [Header("��ġ")]
    public int getExpAmount;

    public override void GetReward()
    {
        GameManager.Instance.player.GetExp(getExpAmount); // 5���� �������� -> ���� ����ġ �����°� 1����
    }
}