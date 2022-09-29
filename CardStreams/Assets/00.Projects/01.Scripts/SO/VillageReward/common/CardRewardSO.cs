using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardReward", menuName = "SO/VillageReward/Card")]
public class CardRewardSO : VillageRewardSO
{
    [Header("��������")]
    public List<SpecialCardSO> randSpecialList; // Ư��ī�� ���� �̱�
    public List<BuildSO> randBuildList; // �ǹ� ���� �̱�

    public override void GetReward()
    {
        bool giveSpecialCard = Random.Range(0, 2) == 0 ? true : false;

        if(giveSpecialCard)
        {
            int rand = Random.Range(0, randSpecialList.Count);
            GameManager.Instance.handleController.AddSpecial(randSpecialList[rand].id);
        }
        else
        {
            int rand = Random.Range(0, randBuildList.Count);
            GameManager.Instance.handleController.AddBuild(randBuildList[rand].id);
        }
    }
}
