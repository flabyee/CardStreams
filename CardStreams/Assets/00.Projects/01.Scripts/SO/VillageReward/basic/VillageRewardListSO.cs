using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageRewardListSO", menuName = "SO/VillageReward/List")]
public class VillageRewardListSO : ScriptableObject
{
    public List<VillageRewardSO> rewardList;
}
