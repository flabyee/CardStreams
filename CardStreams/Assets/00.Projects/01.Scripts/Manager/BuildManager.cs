using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 어떠한 시점(예시 : TurnEnd)때 Access되는 건물을 사용하려고?
public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public Action OnBuildWhenTurnStart;
    public Action OnBuildWhenTurnEnd;
    public Action OnBuildWhenMoveStart;
    public Action OnBuildWhenMoveEnd;

    private void Awake()
    {
        Instance = this;
    }

    public void TurnStart()
    {
        OnBuildWhenTurnStart?.Invoke();
    }

    public void TurnEnd()
    {
        OnBuildWhenTurnEnd?.Invoke();
    }

    public void MoveStart()
    {
        OnBuildWhenMoveStart?.Invoke();
    }

    public void MoveEnd()
    {
        OnBuildWhenMoveEnd?.Invoke();
    }
}
