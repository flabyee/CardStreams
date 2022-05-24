using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopRewardListSO", menuName = "ScriptableObject/Reward/LoopRewardList")]
public class LoopRewardListSO : ScriptableObject
{
    public List<RewardListSO> loopList;
}
