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
    Exp,
    Crystal,
    Prestige,
}

[CreateAssetMenu(fileName = "new Reward", menuName = "ScriptableObject/Mission/MissionReward")]
public class MissionRewardSO : ScriptableObject
{
    public MissionGrade grade;

    public string rewardStr;

    // type에 따라 value의 역할이 달라짐, 예를 들어 카드면 카드의 등급 0~4 => common ~ legendary
    public RewardType rewardType;
    public int value;
}
