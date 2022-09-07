using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriestNPC", menuName = "ScriptableObject/NPC/Priest")]
public class PriestNPCSO : NpcSO
{
    public override void AccessPlayer(Player player)
    {
        Debug.Log("�ִ�ü���� �þ���ϴ�");
        player.hpValue.RuntimeMaxValue += 2;
    }
}
