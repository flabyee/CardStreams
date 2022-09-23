using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Missions/LimitUsePlayerCardMission")]
public class LimitUsePlayerCardMissionSO : MissionSO
{
    public List<BasicType> limitTypes;
    private int curCount;
    public int limitCount;

    public override void GetMission()
    {
        MissionObserverManager.instance.OnBasicCard += ObserverUseBasicCard;

        curCount = 0;
    }

    public override void ApplyUI()
    {
        progressText.text = $"{curCount}/{limitCount}";
        progressSlider.value = Mathf.Clamp((float)curCount / (float)limitCount, 0, 1f);

        if (IsComplete())
        {
            progressText.color = Color.green;
        }
        else if(curCount >= limitCount)
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

    private void ObserverUseBasicCard(BasicCard basicCard)
    {
        if(limitTypes.Contains(basicCard.basicType))
        {
            curCount++;
            ApplyUI();
        }
    }

    public override void Reset()
    {
        MissionObserverManager.instance.OnBasicCard -= ObserverUseBasicCard;
    }
}
