using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoreExpPassive", menuName = "ScriptableObject/Passive/MoreExp")]

public class MoreExpPassiveSO : PassiveSO
{
    public override void Init(Passive passive)
    {
        base.Init(passive);
        MissionObserverManager.instance.OnBasicCard += TriggerPassive;
    }

    private void TriggerPassive(BasicCard card)
    {
        if (card.basicType == BasicType.Monster)
        {
            GameManager.Instance.player.GetExp(currentLevel);
        }
    }
}
