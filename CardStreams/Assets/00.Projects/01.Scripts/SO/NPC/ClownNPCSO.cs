using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClownNPC", menuName = "ScriptableObject/NPC/Clown")]
public class ClownNPCSO : NpcSO
{
    public IntValue startExpValue;

    public int addExpAmount;

    public override void AccessPlayer(Player player)
    {
        base.AccessPlayer(player);

        startExpValue.RuntimeValue += addExpAmount;
    }
}
