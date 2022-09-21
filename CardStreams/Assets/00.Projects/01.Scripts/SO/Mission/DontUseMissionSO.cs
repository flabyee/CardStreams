using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Missions/DontUseMission")]
public class DontUseMissionSO : MissionSO
{
    public List<BasicType> limitTypes;
    private int curCount;
    public int limitCount;

    public override void GetMission()
    {
        GameManager.Instance.player.OnBasicCardEvent += ObserverUseBasicCard;

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
        Debug.Log("observer");

        if(limitTypes.Contains(basicCard.basicType))
        {
            curCount++;
            ApplyUI();
            Debug.Log($"{curCount}/{limitCount}");
        }
    }

    public override void Reset()
    {
        GameManager.Instance.player.OnBasicCardEvent -= ObserverUseBasicCard;
    }
}
