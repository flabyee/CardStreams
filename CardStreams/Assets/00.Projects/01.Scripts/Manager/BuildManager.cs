using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ��� ����(���� : TurnEnd)�� Access�Ǵ� �ǹ��� ����Ϸ���?
public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public Action OnBuildWhenTurnStart;
    public Action OnBuildWhenTurnEnd;
    public Action OnBuildWhenMoveStart;
    public Action OnBuildWhenMoveEnd;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
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
