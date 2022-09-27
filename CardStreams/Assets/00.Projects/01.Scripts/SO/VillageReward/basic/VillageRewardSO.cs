using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VillageRewardSO : ScriptableObject
{
    // �����Ұ� : ��°�(��, ����ġ, ī�尰����), RewardType(Transform ����), �ʿ��ϴٸ� Event

    public VillageRewardListSO soList;

    public VillageRewardType rewardType;
    public Sprite rewardSprite;
    public string rewardName;
    


    public void AddReward()
    {
        soList.rewardList.Add(this);
    }

    public abstract void GetReward(); // ����Դ°� �ϰ��˾Ƽ������ϼ���
}