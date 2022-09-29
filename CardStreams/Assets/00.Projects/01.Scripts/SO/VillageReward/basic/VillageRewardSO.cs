using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VillageRewardSO : ScriptableObject
{
    // �����Ұ� : ��°�(��, ����ġ, ī�尰����), RewardType(Transform ����), �ʿ��ϴٸ� Event

    public VillageRewardListSO soList;
    public string rewardName;
    public VillageRewardType rewardType;
    public Sprite rewardSprite;

    public virtual void AddReward()
    {
        VillageUIManager.Instance.rewardPanel.AddReward(this);
        soList.rewardList.Add(this);
    }

    public abstract void GetReward(); // ����Դ°� �ϰ��˾Ƽ������ϼ���
}