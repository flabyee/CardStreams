using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicianNPC", menuName = "ScriptableObject/NPC/Magician")]
public class MagicianNPCSO : NpcSO
{
    public int createShieldAmount;

    //public override void AccessPlayer(Player player)
    //{
    //    base.AccessPlayer(player);

    //    player.shieldValue.RuntimeMaxValue += createShieldAmount;
    //}
}
