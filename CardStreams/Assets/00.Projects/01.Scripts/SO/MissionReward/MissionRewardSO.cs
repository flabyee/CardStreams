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

    // type�� ���� value�� ������ �޶���, ���� ��� ī��� ī���� ��� 0~4 => common ~ legendary
    public RewardType rewardType;
    public int value;
}
