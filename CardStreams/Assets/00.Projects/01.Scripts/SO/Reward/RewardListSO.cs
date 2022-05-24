using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardListSO", menuName = "ScriptableObject/Reward/RewardList")]
public class RewardListSO : ScriptableObject
{
    public List<RewardSO> rewardList;
}
