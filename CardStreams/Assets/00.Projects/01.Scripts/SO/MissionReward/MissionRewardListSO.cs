using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/MissionRewardList")]
public class MissionRewardListSO : ScriptableObject
{
    public List<MissionRewardSO> rewardList;
}
