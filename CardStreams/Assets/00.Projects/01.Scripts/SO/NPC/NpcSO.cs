using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSO : ScriptableObject
{
    public Sprite npcSprite;
    public string npcName;
    public VillageRewardSO rewardSO;

    public void CreateCard(Vector3 startPos)
    {
        EffectManager.Instance.GetBezierCardEffect(startPos, rewardSO.rewardSprite, VillageUIManager.Instance.boxTrm, () => rewardSO.AddReward());
    }
}
