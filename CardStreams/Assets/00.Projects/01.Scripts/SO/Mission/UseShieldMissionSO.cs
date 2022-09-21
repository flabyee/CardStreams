using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/UseShieldMission")]
public class UseShieldMissionSO : MissionSO
{
    private int curCount;
    public int targetCount;

    public override void GetMission()
    {
        throw new System.NotImplementedException();
    }

    public override void ApplyUI()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsComplete()
    {
        throw new System.NotImplementedException();
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
