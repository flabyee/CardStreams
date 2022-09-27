using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MerchantNPC", menuName = "ScriptableObject/NPC/Merchant")]
public class MerchantNPCSO : NpcSO
{
    public BuildSO[] randomBuildList;
    public SpecialCardSO[] randomCardList;

    public BuildListSO merchantBuildList;
    public SpecialCardListSO merchantCardList;

    public override void AccessPlayer(Player player)
    {
        base.AccessPlayer(player);

        // 지정된 카드를 주기
        int randomIndex = Random.Range(0, randomBuildList.Length + randomCardList.Length);
        if(randomIndex < randomBuildList.Length) // 랜덤돌린게 buildList Length 이상이면 cardList로 넘어가기
        {
            merchantBuildList.buildList.Add(randomBuildList[randomIndex]);
        }
        else
        {
            merchantCardList.specialCardList.Add(randomCardList[randomIndex - randomBuildList.Length]);
        }
    }
}
