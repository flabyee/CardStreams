using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Missions/UseShieldMission")]
public class UseShieldMissionSO : MissionSO
{
    private int curCount;
    public int targetCount;

    public override void GetMission()
    {
        MissionObserverManager.instance.OnShield += ObserverUseShield;

        curCount = 0;
    }

    public override void ApplyUI()
    {
        progressText.text = $"{curCount}/{targetCount}";
        progressSlider.value = Mathf.Clamp((float)curCount / (float)targetCount, 0, 1f);

        if (IsComplete())
        {
            progressText.color = Color.green;
        }
        else
        {
            progressText.color = Color.white;
        }
    }

    public override bool IsComplete()
    {
        if (curCount >= targetCount)
            return true;
        else
            return false;
    }

    private void ObserverUseShield(int value)
    {
        curCount += value;
        ApplyUI();
    }

    public override void Reset()
    {
        MissionObserverManager.instance.OnShield -= ObserverUseShield;
    }
}
