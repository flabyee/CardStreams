using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Reward", menuName = "ScriptableObject/Mission/MissionReward")]
public class MissionRewardSO : ScriptableObject
{
    public MissionGrade grade;
    
    public virtual void GetReward()
    {
        Debug.Log("Get Reward");
    }
}
