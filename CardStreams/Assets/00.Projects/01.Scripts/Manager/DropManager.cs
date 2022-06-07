using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropManager : MonoBehaviour
{
    public DropArea handleDropArea;

    public DropArea buildHandleDropArea;

    public DropArea[] quickSlotDropAreaArr;

    public DropArea shopDropArea;

    public DropArea rerollDropArea;

    public DropArea useDropArea;

    public DropArea hoverDropArea;
    public RectTransform hoverTrm;

    public EventSO rerollEvent;

    private void Start()
    {
        foreach (DropArea dropArea in DropArea.dropAreas)
        {
            switch (dropArea.dropAreaType)
            {
                case DropAreaType.feild:
                    dropArea.onLifted += ObjectLiftedFromFeild;
                    dropArea.onDropped += ObjectDroppedToFeild;
                    break;
                case DropAreaType.build:
                    dropArea.onLifted += ObjectLiftedFromBuild;
                    dropArea.onDropped += ObjectDroppedToBuild;
                    break;
            }
        }


        handleDropArea.onLifted += ObjectLiftedFromHandle;
        handleDropArea.onDropped += ObjectDroppedToHandle;

        buildHandleDropArea.onLifted += ObjectLiftedFromBuildHandle;
        buildHandleDropArea.onDropped += ObjectDroppedToBuildHandle;

        foreach(DropArea dropArea in quickSlotDropAreaArr)
        {
            dropArea.onLifted += ObjectLiftedFromQuickSlot;
            dropArea.onDropped += ObjectDroppedToQuickSlot;
        }

        shopDropArea.onLifted += ObjectLiftedFromShop;
        shopDropArea.onDropped += ObjectDroppedToShop;

        rerollDropArea.onLifted += ObjectLiftedFromReroll;
        rerollDropArea.onDropped += ObjectDroppedToReroll;

        useDropArea.onLifted += ObjectLiftedFromUse;
        useDropArea.onDropped += ObjectDroppedToUse;

        hoverDropArea.onLifted += ObjectLiftedFromHover;
        hoverDropArea.onDropped += ObjectDroppedToHover;
    }

    private void ObjectLiftedFromFeild(DropArea area, GameObject obj)
    {
        obj.transform.SetParent(hoverTrm, true);

        area.field.ResetData();

        // fieldType ����
        //area.field.fieldType = FieldType.able;
    }
    private void ObjectDroppedToFeild(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        // field�� ���°� �ƴ϶��(���� : �ǹ�) �ٽ� ����ġ
        if (cardPower.cardType == CardType.Build || cardPower.cardType == CardType.Special)
        {
            ObjectToOrigin(area, obj);
        }

        // ī�尡 �Ϲ� ī����
        if (cardPower.cardType != CardType.Special && cardPower.cardType != CardType.Build)
        {
            // 1. ��ġ�����Ѱ� ���� 3,  area.field.fieldType == FieldType.randomMob �̰Ŵ� ���ϳ�?
            // �ϴ� ���� : ó���� ��� �ʵ��� ���°� not �̱� ������
            if (area.field.fieldState == FieldState.able)
            {
                // 2.�̹� ���� ��ġ�Ǿ��ִ��� Ȯ��, 
                if (area.field.cardPower == null || area.field.cardPower.cardType == CardType.NULL)
                {
                    // �θ� ����
                    obj.transform.SetParent(area.rectTrm, true);

                    // ���� ����
                    area.field.cardPower = cardPower;
                    area.field.dragbleCard = dragbleCard;

                    dragbleCard.IsField();

                    // fieldType ����
                    //area.field.fieldType = FieldType.not;
                }
                else
                {
                    // �������� �Ѱ��� �ִ� ī���� ������̾� ���
                    DropArea myDropArea = dragbleCard.prevDropArea;  // ���� �ִ� dropArea
                    DropArea changeDropArea = area;                 // ����� ���� dropArea
                    DragbleCard otherDragbleCard = changeDropArea.rectTrm.GetChild(0).GetComponent<DragbleCard>();

                    if (otherDragbleCard == null)
                    {
                        Debug.Log("is null");
                    }

                    // �װ� lift
                    changeDropArea.TriggerOnLift(otherDragbleCard);

                    // drop drop
                    myDropArea.TriggerOnDrop(otherDragbleCard);
                    changeDropArea.TriggerOnDrop(dragbleCard);
                }
            }
            // ���� ��ġ���ְų� ��ġ �ȵǴ� ���̸�
            else
            {
                // ���ڸ���
                ObjectToOrigin(area, obj);
            }
        }
        // ����� ī����? 
        else
        {
            // �ϴ� �ѹ��� Ȯ��, �ֳ��ϸ� �ٸ� ī�嵵 �߰��ɼ��־?
            if (cardPower.cardType == CardType.Special)
            {
                SpecialCard specialCard = obj.GetComponent<SpecialCard>();



                // targetType �´°� �ִ��� Ȯ��
                foreach (CardType targetType in specialCard.targetTypeList)
                {
                    if (area.field.cardPower != null && area.field.cardPower.cardType == targetType)
                    {
                        switch (specialCard.applyTiming)
                        {

                            case ApplyTiming.NowField:
                                specialCard.OnAccessSpecialCard(GameManager.Instance.player, area.field);

                                dragbleCard.isDestory = true;

                                return;
                            case ApplyTiming.OnFeild:
                                //area.feild.accessBeforeOnField += specialCard.OnAccessSpecialCard;
                                break;
                            case ApplyTiming.ToPlayer:
                                break;
                        }

                        // �´°� �ִٸ� ȿ�������ϰ� ����� ī�� ����


                    }
                }

                // �´°� ������ �ٽ� ���ڸ���
                ObjectToOrigin(area, obj);
            }
        }
    }


    private void ObjectLiftedFromBuild(DropArea area, GameObject obj)
    {
        obj.transform.SetParent(hoverTrm, true);
    }
    private void ObjectDroppedToBuild(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        // �ƹ��͵����� �ǹ�ī���� (buildDropArea�� field�� ��� �ڽ��� ������ üũ
        if (cardPower.cardType == CardType.Build && area.rectTrm.childCount == 0)
        {
            // �θ� ����(��ġ ����)
            obj.transform.SetParent(area.rectTrm, true);

            // �������̰� ����
            dragbleCard.canDragAndDrop = false;

            // �ǹ� ȿ�� ����
            BuildCard build = obj.GetComponent<BuildCard>();
            build.BuildDrop(area.point);

        }
        // To Do : �ǹ��νñ� ���߿� ���� ������ �ض�!!
        else if(cardPower.cardType == CardType.Special)
        {
            SpecialCard specialCard = obj.GetComponent<SpecialCard>();

            if(specialCard.targetTypeList[0] == CardType.Build)
            {
                dragbleCard.isDestory = true;

                BuildCard build = area.rectTrm.GetChild(0).GetComponent<BuildCard>();
                specialCard.OnAccessBuildCard(build);
            }
            else
            {
                // ���ڸ���
                ObjectToOrigin(area, obj);
            }
        }
        else
        {
            // ���ڸ���
            ObjectToOrigin(area, obj);
        }
    }

    private void ObjectLiftedFromQuickSlot(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();

        obj.transform.SetParent(hoverTrm, true);

        if(GameManager.Instance.curState == GameState.Equip)
        {
            dragbleCard.originDropArea = dragbleCard.originOriginDropArea;
        }
    }
    private void ObjectDroppedToQuickSlot(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        // �ƹ��͵����� Ư��ī���� (buildDropArea�� field�� ��� �ڽ��� ������ üũ
        if (cardPower.cardType == CardType.Special && area.rectTrm.childCount == 0)
        {
            // �θ� ����(��ġ ����)
            obj.transform.SetParent(area.rectTrm, true);

            Debug.Log(dragbleCard.originDropArea);

            dragbleCard.originOriginDropArea = dragbleCard.originDropArea;
            dragbleCard.originDropArea = area;

            Debug.Log(dragbleCard.originDropArea);
        }
        else
        {
            // ���ڸ���
            ObjectToOrigin(area, obj);
        }
    }

    private void ObjectLiftedFromHandle(DropArea area, GameObject obj)
    {
        obj.transform.SetParent(hoverTrm, true);
    }
    private void ObjectDroppedToHandle(DropArea area, GameObject obj)
    {
        ObjectToOrigin(area, obj);
    }


    private void ObjectLiftedFromBuildHandle(DropArea area, GameObject obj)
    {
        obj.transform.SetParent(hoverTrm, true);
    }
    private void ObjectDroppedToBuildHandle(DropArea area, GameObject obj)
    {
        ObjectToOrigin(area, obj);
    }


    private void ObjectLiftedFromShop(DropArea area, GameObject obj)
    {

    }
    private void ObjectDroppedToShop(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        if (cardPower.cardType == CardType.Basic && (cardPower as BasicCard).basicType != BasicType.Monster)
        {
            GameManager.Instance.AddGold(2);
            //dragbleCard.isDestory = true;

            (cardPower as BasicCard).SetValue(0);
            (cardPower as BasicCard).ApplyUI();

            //ObjectToOrigin(area, obj);

            bool b = GameManager.Instance.DropField(dragbleCard);
            if(b == false)
            {
                ObjectToOrigin(area, obj);
            }
        }
        else
        {
            ObjectToOrigin(area, obj);
        }
    }


    private void ObjectLiftedFromReroll(DropArea area, GameObject obj)
    {

    }
    private void ObjectDroppedToReroll(DropArea area, GameObject obj)
    {
        //DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        //CardPower cardPower = obj.GetComponent<CardPower>();
        //if (cardPower.dropAreaType == DropAreaType.feild)
        //{
        //    rerollEvent.Occurred(obj);
        //    dragbleCard.isDestory = true;

        //    GameManager.Instance.RerollScore();
        //}
        //else
        //{
        //    ObjectToOrigin(area, obj);
        //}
    }

    private void ObjectLiftedFromUse(DropArea area, GameObject obj)
    {

    }

    private void ObjectDroppedToUse(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        if (cardPower.cardType != CardType.Special)
        {
            ObjectToOrigin(area, obj);
            return;
        }

        SpecialCard specialCard = obj.GetComponent<SpecialCard>();

        switch (specialCard.applyTiming)
        {
            case ApplyTiming.NULL:
                break;

            case ApplyTiming.ToPlayer:
                specialCard.OnAccessSpecialCard(GameManager.Instance.player, null);
                dragbleCard.isDestory = true;
                return;

            case ApplyTiming.NowField:
                break;

            case ApplyTiming.OnFeild:

                break;

            default:
                break;
        }

        ObjectToOrigin(area, obj);
    }

    private void ObjectLiftedFromHover(DropArea area, GameObject obj)
    {

    }
    private void ObjectDroppedToHover(DropArea area, GameObject obj)
    {
        //Debug.Log("hover drop");
        ObjectToOrigin(area, obj);
    }


    private void ObjectToOrigin(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        //Debug.Log(dragbleCard.cardPower.cardType);

        obj.transform.SetParent(dragbleCard.originDropArea.rectTrm, true);
        dragbleCard.SetDroppedArea(dragbleCard.originDropArea);

        dragbleCard.SetHandle();
    }
}