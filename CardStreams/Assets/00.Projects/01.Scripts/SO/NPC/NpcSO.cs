using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "SO/NPC")]
public class NpcSO : ScriptableObject
{
    public Sprite npcSprite;
    public VillageRewardSO rewardSO;

    public void CreateCard(Vector3 startPos)
    {
        EffectManager.Instance.CreateBezierEffect(startPos, rewardSO.rewardSprite, VillageUIManager.Instance.boxTrm, null);
        rewardSO.AddReward();
    }
}
