using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuildSO : BuildSO
{
    // 상속해서 여기를 채운다
    public override void AccessCard(Field field)
    {

    }

    // 상속해서 여기를 채운다
    public override void AccessPlayer(Player player)
    {
        Debug.Log("적 설치 건물에 플레이어 인접");
    }

    public override void AccessTurnEnd(Vector3 buildPos)
    {

    }
}
