using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicianNPC", menuName = "ScriptableObject/NPC/Magician")]
public class MagicianNPCSO : NpcSO
{
    public VillageBuffListSO buffListSO;
    public TurnRecoveryBuffSO buffSO;

    public override void AccessPlayer(Player player)
    {
        buffListSO.buffList.Add(buffSO);
    }
}
