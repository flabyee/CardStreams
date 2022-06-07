using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum FieldState
{
    NULL,
    yet,    // ���� ��ġ���� -> able
    able,   // ���� ��ġ������ -> ��ġ�ϸ� not
    not,    // ��ġ �Ұ�����, �������ڸ��� ���� �ɶ��� �ִ�
    randomMob,  // �����ϰ� ���� �����ϰڴ� <- ������ �Ⱦ��µ�?
}

// 1. ��� yet 2. not ����, 3. �������� able 
//    able �ΰ����� ��ġ����

public class Field : MonoBehaviour
{
    public Image background;

    public FieldState fieldState = FieldState.NULL;


    public DragbleCard dragbleCard;
    public CardPower cardPower;

    public Action<Player> accessBuildToPlayerAfterOnField;
    public Action<Player, Field> accessBeforeOnField;
    public Action<Field> accessBuildToCardAfterMoveStart;

    [Header("Debug")]
    public DropArea dropArea;

    public void Init(CardPower power, DragbleCard dragCard, FieldState type)
    {
        cardPower = power;
        dragbleCard = dragCard;
        fieldState = type;
    }

    public void ResetData()
    {
        cardPower = null;
        dragbleCard = null;
    }
    public void OnBuildAccess()
    {
        accessBuildToCardAfterMoveStart?.Invoke(this);
    }

    public void FieldReset()
    {
        if(dragbleCard != null)
        {
            Debug.Log("reset");
            dragbleCard.ActiveFalse();
        }

        ResetData();
    }

}
