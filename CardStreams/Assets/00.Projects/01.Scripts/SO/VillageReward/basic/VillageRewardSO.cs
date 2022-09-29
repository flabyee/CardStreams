using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VillageRewardSO : ScriptableObject
{
    // 들어가야할거 : 얻는것(돈, 경험치, 카드같은거), RewardType(Transform 구분), 필요하다면 Event

    public VillageRewardListSO soList;
    public string rewardName;
    public VillageRewardType rewardType;
    public Sprite rewardSprite;

    public virtual void AddReward()
    {
        VillageUIManager.Instance.rewardPanel.AddReward(this);
        soList.rewardList.Add(this);
    }

    public abstract void GetReward(); // 보상먹는건 니가알아서구현하세요
}