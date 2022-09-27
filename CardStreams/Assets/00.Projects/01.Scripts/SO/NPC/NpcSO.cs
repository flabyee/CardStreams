using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSO : ScriptableObject
{
    public Sprite npcSprite;
    public string npcName;
    public VillageRewardSO rewardSO;

    public virtual void AccessPlayer(Player player)
    {

    }
}
