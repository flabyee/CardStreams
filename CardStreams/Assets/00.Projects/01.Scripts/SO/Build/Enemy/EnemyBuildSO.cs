using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuildSO : BuildSO
{
    // ����ؼ� ���⸦ ä���
    public override void AccessCard(Field field)
    {

    }

    // ����ؼ� ���⸦ ä���
    public override void AccessPlayer(Player player)
    {
        Debug.Log("�� ��ġ �ǹ��� �÷��̾� ����");
    }

    public override void AccessTurnEnd(Vector3 buildPos)
    {

    }
}
