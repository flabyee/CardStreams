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

        // ������ ī�带 �ֱ�
        int randomIndex = Random.Range(0, randomBuildList.Length + randomCardList.Length);
        if(randomIndex < randomBuildList.Length) // ���������� buildList Length �̻��̸� cardList�� �Ѿ��
        {
            merchantBuildList.buildList.Add(randomBuildList[randomIndex]);
        }
        else
        {
            merchantCardList.specialCardList.Add(randomCardList[randomIndex - randomBuildList.Length]);
        }
    }
}
