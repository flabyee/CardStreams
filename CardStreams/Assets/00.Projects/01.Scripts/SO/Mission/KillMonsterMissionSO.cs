using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Missions/KillMonsterMission")]
public class KillMonsterMissionSO : MissionSO
{
    private int curCount;
    public int targetCount;

    public override void GetMission()
    {
        GameManager.Instance.player.OnBasicCardEvent += ObserverUseBasicCard;

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

    private void ObserverUseBasicCard(BasicCard basicCard)
    {
        if (basicCard.basicType == BasicType.Monster)
        {
            curCount++;
            ApplyUI();
        }
    }

    public override void Reset()
    {
        GameManager.Instance.player.OnBasicCardEvent -= ObserverUseBasicCard;
    }
}
