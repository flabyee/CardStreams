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

        // fieldType 설정
        //area.field.fieldType = FieldType.able;
    }
    private void ObjectDroppedToFeild(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        // field에 놓는게 아니라면(예시 : 건물) 다시 원위치
        if (cardPower.cardType == CardType.Build || cardPower.cardType == CardType.Special)
        {
            ObjectToOrigin(area, obj);
        }

        // 카드가 일반 카드라면
        if (cardPower.cardType != CardType.Special && cardPower.cardType != CardType.Build)
        {
            // 1. 설치가능한곳 인지 3,  area.field.fieldType == FieldType.randomMob 이거는 왜하냐?
            // 하는 이유 : 처음에 모든 필드의 상태가 not 이기 때문에
            if (area.field.fieldState == FieldState.able)
            {
                // 2.이미 뭐가 배치되어있는지 확인, 
                if (area.field.cardPower == null || area.field.cardPower.cardType == CardType.NULL)
                {
                    // 부모 설정
                    obj.transform.SetParent(area.rectTrm, true);

                    // 정보 설정
                    area.field.cardPower = cardPower;
                    area.field.dragbleCard = dragbleCard;

                    dragbleCard.IsField();

                    // fieldType 설정
                    //area.field.fieldType = FieldType.not;
                }
                else
                {
                    // 놓으려고 한곳의 있던 카드의 드랍에이어 얻기
                    DropArea myDropArea = dragbleCard.prevDropArea;  // 내가 있던 dropArea
                    DropArea changeDropArea = area;                 // 드랍한 곳의 dropArea
                    DragbleCard otherDragbleCard = changeDropArea.rectTrm.GetChild(0).GetComponent<DragbleCard>();

                    if (otherDragbleCard == null)
                    {
                        Debug.Log("is null");
                    }

                    // 그거 lift
                    changeDropArea.TriggerOnLift(otherDragbleCard);

                    // drop drop
                    myDropArea.TriggerOnDrop(otherDragbleCard);
                    changeDropArea.TriggerOnDrop(dragbleCard);
                }
            }
            // 뭐가 배치되있거나 설치 안되는 곳이면
            else
            {
                // 재자리로
                ObjectToOrigin(area, obj);
            }
        }
        // 스페셜 카드라면? 
        else
        {
            // 일단 한번더 확인, 왜냐하면 다른 카드도 추가될수있어서?
            if (cardPower.cardType == CardType.Special)
            {
                SpecialCard specialCard = obj.GetComponent<SpecialCard>();



                // targetType 맞는거 있는지 확인
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

                        // 맞는게 있다면 효과적용하고 스페셜 카드 삭제


                    }
                }

                // 맞는게 없으면 다시 재자리로
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

        // 아무것도없고 건물카드라면 (buildDropArea는 field가 없어서 자식의 갯수로 체크
        if (cardPower.cardType == CardType.Build && area.rectTrm.childCount == 0)
        {
            // 부모 설정(위치 설정)
            obj.transform.SetParent(area.rectTrm, true);

            // 못움직이게 설정
            dragbleCard.canDragAndDrop = false;

            // 건물 효과 적용
            BuildCard build = obj.GetComponent<BuildCard>();
            build.BuildDrop(area.point);

        }
        // To Do : 건물부시기 나중에 개선 무조건 해라!!
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
                // 재자리로
                ObjectToOrigin(area, obj);
            }
        }
        else
        {
            // 재자리로
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

        // 아무것도없고 특수카드라면 (buildDropArea는 field가 없어서 자식의 갯수로 체크
        if (cardPower.cardType == CardType.Special && area.rectTrm.childCount == 0)
        {
            // 부모 설정(위치 설정)
            obj.transform.SetParent(area.rectTrm, true);

            Debug.Log(dragbleCard.originDropArea);

            dragbleCard.originOriginDropArea = dragbleCard.originDropArea;
            dragbleCard.originDropArea = area;

            Debug.Log(dragbleCard.originDropArea);
        }
        else
        {
            // 재자리로
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