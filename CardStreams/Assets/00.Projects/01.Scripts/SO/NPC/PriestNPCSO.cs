using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriestNPC", menuName = "ScriptableObject/NPC/Priest")]
public class PriestNPCSO : NpcSO
{
    public override void AccessPlayer(Player player)
    {
        Debug.Log("최대체력이 늘어났습니다");
        player.hpValue.RuntimeMaxValue += 2;
    }
}
