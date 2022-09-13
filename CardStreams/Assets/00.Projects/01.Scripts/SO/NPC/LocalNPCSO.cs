using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalNPC", menuName = "ScriptableObject/NPC/Local")]
public class LocalNPCSO : NpcSO
{
    public override void AccessPlayer(Player player)
    {
        Debug.Log("���� ���� �þ���ϴ�");
        player.goldValue.RuntimeValue += 10;
    }
}
