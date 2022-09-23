using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Missions/LimitTimeMission")]
public class LimitTimeMissionSO : MissionSO
{
    public int limitTime;
    private int nowTime;

    public override void GetMission()
    {
        MissionObserverManager.instance.TimerEvent += ObserverTimer;

        nowTime = 0;
    }

    public override void ApplyUI()
    {
        progressText.text = $"{nowTime}/{limitTime}";
        progressSlider.value = Mathf.Clamp((float)nowTime / (float)limitTime, 0, 1f);

        if (IsComplete())
        {
            progressText.color = Color.green;
        }
        else
        {
            progressText.color = Color.red;
        }
    }

    public override bool IsComplete()
    {
        if (nowTime <= limitTime)
            return true;
        else
            return false;
    }

    public override void Reset()
    {
        MissionObserverManager.instance.TimerEvent -= ObserverTimer;
    }

    private void ObserverTimer(int time)
    {
        nowTime = time;
        ApplyUI();
    }
}
