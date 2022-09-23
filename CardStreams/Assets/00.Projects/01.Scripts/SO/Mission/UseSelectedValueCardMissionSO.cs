using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Missions/UseSelectedValueCardMission")]
public class UseSelectedValueCardMissionSO : MissionSO
{
    public List<BasicType> useTypes;
    public int targetValue;

    private bool isClear;

    public override void GetMission()
    {
        MissionObserverManager.instance.OnBasicCard += ObserverUseBasicCard;

        isClear = false;
    }

    public override void ApplyUI()
    {
        progressText.text = $"{(isClear ? 1 : 0)}/{1}";
        progressSlider.value = (isClear ? 1 : 0);

        if (IsComplete())
        {
            progressText.color = Color.green;
        }
        else
        {
            progressText.color = Color.white;
        }
    }

    private void ObserverUseBasicCard(BasicCard basicCard)
    {
        Debug.Log("observer");

        if (useTypes.Contains(basicCard.basicType))
        {
            if(targetValue == basicCard.value)
            {
                isClear = true;
                ApplyUI();
            }
        }
    }

    public override bool IsComplete()
    {
        return isClear;
    }

    public override void Reset()
    {
        MissionObserverManager.instance.OnBasicCard -= ObserverUseBasicCard;
    }
}
