using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType
{
    Gold,
    Hp,
    MaxHp,
    BuildCard,
    SpecialCard,
}

[CreateAssetMenu(fileName = "new Reward", menuName = "ScriptableObject/Mission/MissionReward")]
public class MissionRewardSO : ScriptableObject
{
    public MissionGrade grade;

    public string rewardStr;

    // type에 따라 value의 역할이 달라짐, 예를 들어 카드면 카드의 ID
    public RewardType rewardType;
    public int value;
}
