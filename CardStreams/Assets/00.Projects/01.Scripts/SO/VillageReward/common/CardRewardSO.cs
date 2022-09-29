using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardReward", menuName = "SO/VillageReward/Card")]
public class CardRewardSO : VillageRewardSO
{
    [Header("¼³Á¤º¯¼ö")]
    public List<SpecialCardSO> randSpecialList; // Æ¯¼öÄ«µå ·£´ý »Ì±â
    public List<BuildSO> randBuildList; // °Ç¹° ·£´ý »Ì±â

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
