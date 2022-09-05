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

    // ��� �� ��������
    public void SetAllFieldYet()
    {
        foreach (Field field in MapManager.Instance.fieldList)
        {
            field.fieldState = FieldState.yet;
        }
    }

    // ������ Ȱ��ȭ
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

    // ������ ��Ȱ��ȭ
    public void SetBeforeFieldNot(int nowIndex)
    {
        for (int i = nowIndex - 4; i < nowIndex; i++)
        {
            // (fieldType = not)
            MapManager.Instance.SetFieldState(i, FieldState.not);
        }
    }

    public void SetMonsterGoldP(int nowIndex)
    {
        int mobCount = 0;

        for (int i = nowIndex - 4; i < nowIndex; i++)
        {
            if(MapManager.Instance.fieldList[i].cardPower.basicType == BasicType.Monster)
            {
                mobCount++;
            }
        }

        int goldP = 1;

        if(mobCount == 3)
            goldP = 2;
        else if(mobCount == 4)
        {
            goldP = 3;
        }

        for (int i = nowIndex - 4; i < nowIndex; i++)
        {
            if (MapManager.Instance.fieldList[i].cardPower.basicType == BasicType.Monster)
            {
                MapManager.Instance.fieldList[i].cardPower.goldP *= goldP;
            }
        }
    }

    // �ǹ� ȿ�� ����
    //public void BuildAccessNextField(int nowIndex)
    //{
    //    for (int i = nowIndex; i < nowIndex + 4; i++)
    //    {
    //        if(MapManager.Instance.fieldList[i].isSet == true)
    //            MapManager.Instance.fieldList[i].OnAcceseBuildToCard();
    //    }
    //}
    

    // �տ� 4ĭ�� �� á����
    public bool IsNextFieldFull(int nowIndex)
    {
        for (int i = nowIndex; i < nowIndex + 4; i++)
        {
            if (MapManager.Instance.fieldList[i].isSet == false)
            {
                return false;
            }
        }

        return true;
    }

    // �տ� 4ĭ�� �÷��̾�ī�尡 2�� ��������
    public bool IsNextFieldPlayerCardTwo(int nowIndex)
    {
        int playerCardCount = 0;

        for (int i = nowIndex; i < nowIndex + 4; i++)
        {
            Field field = MapManager.Instance.fieldList[i];
            if (field.isSet == true && (field.cardPower as BasicCard).basicType != BasicType.Monster)
            {
                playerCardCount++;
            }
        }
        return playerCardCount <= 2 ? true : false;
    }

    // �� 4ĭ�� ����ִ� �� ����Ʈ
    public List<Vector2> GetTempNextFieldPoint()
    {
        List<Vector2> tempPointList = new List<Vector2>();

        for (int i = GameManager.Instance.moveIndex; i < GameManager.Instance.moveIndex + 4; i++)
        {
            if (MapManager.Instance.fieldList[i].isSet == false)
            {
                tempPointList.Add(MapManager.Instance.fieldList[i].dropArea.point);
            }
        }

        return tempPointList;
    }
}
