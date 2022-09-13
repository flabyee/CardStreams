using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalNPC", menuName = "ScriptableObject/NPC/Local")]
public class LocalNPCSO : NpcSO
{
    public override void AccessPlayer(Player player)
    {
        Debug.Log("Ω√¿€ µ∑¿Ã ¥√æÓ≥µΩ¿¥œ¥Ÿ");
        player.goldValue.RuntimeValue += 10;
    }
}
