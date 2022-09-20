using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/DontUseMission")]
public class DontUseMission : MissionSO
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

        if (curCount >= limitCount)
        {
            progressText.color = Color.red;
        }
    }

    public override bool IsComplete()
    {
        GameManager.Instance.player.OnBasicCardEvent -= ObserverUseBasicCard;
        UnSetUI();

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
}
