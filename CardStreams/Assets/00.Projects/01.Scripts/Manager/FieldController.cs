using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController
{
    private int maxMoveCount;

    public FieldController(int maxMoveCount)
    {
        this.maxMoveCount = maxMoveCount;
    }

    public void SetAllFieldYet()
    {
        foreach (Field field in MapManager.Instance.fieldList)
        {
            field.fieldState = FieldState.yet;
        }
    }

    public void SetNextFieldAble(int nowIndex)
    {
        for (int i = nowIndex; i < maxMoveCount + nowIndex; i++)
        {
            if (MapManager.Instance.fieldList[i].fieldState == FieldState.yet)
            {
                MapManager.Instance.SetFieldState(i, FieldState.able);
            }
        }
    }

    public void SetBeforeFieldNot(int nowIndex)
    {
        for (int i = nowIndex - 4; i < nowIndex; i++)
        {
            // (fieldType = not)
            MapManager.Instance.SetFieldState(i, FieldState.not);
        }
    }

    public void BuildAccessNextField(int nowIndex)
    {
        for (int i = nowIndex; i < nowIndex + 4; i++)
        {
            MapManager.Instance.fieldList[i].OnBuildAccess();
        }
    }
}
