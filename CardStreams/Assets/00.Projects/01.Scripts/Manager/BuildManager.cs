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

    public List<BuildCard> buildList = new List<BuildCard>();
    public List<ActionPosData> OnBuildWhenTurnEndList = new List<ActionPosData>();

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
        //OnBuildWhenTurnEnd?.Invoke();
        foreach(ActionPosData data in OnBuildWhenTurnEndList)
        {
            data.action(data.obj.transform.position);
        }
    }

    public void MoveStart()
    {
        OnBuildWhenMoveStart?.Invoke();
    }

    public void NextBuildEffect()
    {
        EffectManager.Instance.DeleteNextBuildEffect();

        List<Vector2> nextFieldVectorList = new List<Vector2>();

        int moveIndex = GameManager.Instance.moveIndex;

        for (int i = moveIndex; i < moveIndex + 4; i++)
        {
            nextFieldVectorList.Add(MapManager.Instance.fieldList[i].dropArea.point);
        }

        foreach(BuildCard build in buildList)
        {
            foreach (Vector2 point in build.GetAccessPointList())
            {
                //Debug.Log($"myP : {build.GetMyPoint()}, p : {point}, result : {point + build.GetMyPoint()}");
                Vector2 resultPoint = new Vector2(build.GetMyPoint().x + point.x, build.GetMyPoint().y - point.y);
                if (nextFieldVectorList.Contains(resultPoint))
                {
                    EffectManager.Instance.GetNextBuildEffect(build.transform.position);
                    break;
                }
            }
        }
    }
}

public class ActionPosData
{
    public Action<Vector3> action;
    public GameObject obj;

    public ActionPosData(Action<Vector3> action, GameObject obj)
    {
        this.action = action;
        this.obj = obj;
    }
}
