using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Missions/LimitUseSpecialMission")]
public class LimitUseSpecialMissionSO : MissionSO
{
    private int curCount;
    public int limitCount;

    public override void GetMission()
    {
        curCount = 0;

        MissionObserverManager.instance.OnSpecialCard += ObserverUseSpecialCard;
        MissionObserverManager.instance.OffSpecialCard += ObserverUnUseSpecialCard;
    }

    public override void ApplyUI()
    {
        progressText.text = $"{curCount}/{limitCount}";
        progressSlider.value = Mathf.Clamp((float)curCount / (float)limitCount, 0, 1f);

        if (IsComplete())
        {
            progressText.color = Color.green;
        }
        else if (curCount >= limitCount)
        {
            progressText.color = Color.red;
        }
        else
        {
            progressText.color = Color.white;
        }
    }

    public override bool IsComplete()
    {
        if (curCount < limitCount)
            return true;
        else
            return false;
    }

    private void ObserverUseSpecialCard(int specialCardID)
    {
        curCount++;
        ApplyUI();
    }
    private void ObserverUnUseSpecialCard(int specialCardID)
    {
        curCount--;
        ApplyUI();
    }

    public override void Reset()
    {
        MissionObserverManager.instance.OnSpecialCard -= ObserverUseSpecialCard;
        MissionObserverManager.instance.OffSpecialCard -= ObserverUnUseSpecialCard;
    }
}
