using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum FieldType
{
    NULL,
    yet,    // ���� ��ġ���� -> able
    able,   // ���� ��ġ������ -> ��ġ�ϸ� not
    not,    // ��ġ �Ұ�����, �������ڸ��� ���� �ɶ��� �ִ�
    randomMob,  // �����ϰ� ���� �����ϰڴ�
}

// 1. ��� yet 2. not ����, 3. �������� able 
//    able �ΰ����� ��ġ����

public class Field : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    public FieldType fieldType = FieldType.NULL;

    public DragbleCard dragbleCard;
    public CardPower cardPower;

    public Action<Player> accessBuildToPlayerAfterOnField;
    public Action<Player, Field> accessBeforeOnField;
    public Action<Field> accessBuildToCardAfterMoveStart;

    [Header("Debug")]
    public DropArea dropArea;

    private void Start()
    {
    }


    public void ResetData()
    {
        cardPower = null;
        dragbleCard = null;
    }
    public void OnAccessCard()
    {
        accessBuildToCardAfterMoveStart?.Invoke(this);
    }
    public void OnfieldReset()
    {
        if(transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        ResetData();
    }
}
