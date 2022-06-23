using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropManager : MonoBehaviour
{
    public DropArea handleDropArea;

    public DropArea buildHandleDropArea;

    public DropArea useDropArea;

    public DropArea hoverDropArea;
    public RectTransform hoverTrm;

    public EventSO rerollEvent;

    public GameObject curDragingObj;

    private void Start()
    {
        foreach (DropArea dropArea in DropArea.dropAreas)
        {
            switch (dropArea.dropAreaType)
            {
                case DropAreaType.Feild:
                    dropArea.onLifted += ObjectLiftedFromFeild;
                    dropArea.onDropped += ObjectDroppedToFeild;
                    break;
                case DropAreaType.Build:
                    dropArea.onLifted += ObjectLiftedFromBuild;
                    dropArea.onDropped += ObjectDroppedToBuild;
                    break;
            }
        }


        handleDropArea.onLifted += ObjectLiftedFromHandle;
        handleDropArea.onDropped += ObjectDroppedToHandle;

        buildHandleDropArea.onLifted += ObjectLiftedFromBuildHandle;
        buildHandleDropArea.onDropped += ObjectDroppedToBuildHandle;

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
        if (cardPower.cardType == CardType.Build)
        {
            ObjectToOrigin(area, obj);
            return;
        }

        // 카드가 일반 카드라면
        if (cardPower.cardType != CardType.Special)
        {
            // 1. 설치가능한곳 인지 3,  area.field.fieldType == FieldType.randomMob 이거는 왜하냐?
            // 하는 이유 : 처음에 모든 필드의 상태가 not 이기 때문에
            if (area.field.fieldState == FieldState.able)
            {
                // 2.이미 뭐가 배치되어있는지 확인, 
                if (area.field.isSet == false)
                {
                    // 위치&부모 설정
                    dragbleCard.transform.position = area.field.transform.position;
                    dragbleCard.transform.SetParent(area.field.transform);

                    area.field.Init(cardPower, dragbleCard);

                    cardPower.SetField();

                    (cardPower as BasicCard).OnField();

                    // fieldType 설정
                    //area.field.fieldType = FieldType.not;
                }
                else
                {
                    // 놓으려고 한곳의 있던 카드의 드랍에이어 얻기
                    DropArea myDropArea = dragbleCard.prevDropArea;  // 내가 있던 dropArea
                    DropArea changeDropArea = area;                 // 드랍한 곳의 dropArea
                    DragbleCard otherDragbleCard = changeDropArea.rectTrm.GetChild(1).GetComponent<DragbleCard>();

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
            SpecialCard specialCard = obj.GetComponent<SpecialCard>();

            if (area.field.fieldState != FieldState.able && area.field.fieldState != FieldState.randomMob)
            {
                ObjectToOrigin(area, obj);
                return;
            }

            // targetType 맞는거 있는지 확인
            foreach (CardType targetType in specialCard.targetTypeList)
            {
                if (area.field.isSet == true && area.field.cardPower.cardType == targetType)
                {
                    if (targetType == CardType.Basic)
                    {
                        foreach (BasicType targetBasic in specialCard.targetBasicList)
                        {
                            BasicCard basicPower = area.field.cardPower as BasicCard;

                            if (targetBasic == basicPower.basicType)
                            {
                                if (specialCard.applyTiming == ApplyTiming.NowField)
                                {
                                    specialCard.OnAccessSpecialCard(GameManager.Instance.player, area.field);
                                    (area.field.cardPower as BasicCard).AddSpecial(specialCard.id);

                                    dragbleCard.isDestory = true;

                                    cardPower.SetField();

                                    return;
                                }
                                else
                                {
                                    Debug.LogError("basic에 쓰는건데 nowFile가 아니다??");
                                }
                            }
                        }
                    }
                }
            }

            // 맞는게 없으면 다시 재자리로
            ObjectToOrigin(area, obj);
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
            dragbleCard.transform.position = area.rectTrm.transform.position;
            dragbleCard.transform.SetParent(area.rectTrm.transform);

            // 못움직이게 설정
            dragbleCard.canDragAndDrop = false;

            // 건물 효과 적용
            BuildCard build = obj.GetComponent<BuildCard>();
            build.BuildDrop(area.point);

            build.OnField();

            cardPower.SetField();
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

                cardPower.SetField();
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


    //private void ObjectLiftedFromShop(DropArea area, GameObject obj)
    //{

    //}
    //private void ObjectDroppedToShop(DropArea area, GameObject obj)
    //{
    //    DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
    //    CardPower cardPower = obj.GetComponent<CardPower>();

    //    if (cardPower.cardType == CardType.Basic && (cardPower as BasicCard).basicType != BasicType.Monster)
    //    {
    //        GameManager.Instance.AddGold(2);
    //        //dragbleCard.isDestory = true;

    //        (cardPower as BasicCard).SetValue(0);
    //        (cardPower as BasicCard).ApplyUI();

    //        //ObjectToOrigin(area, obj);

    //        bool b = GameManager.Instance.DropField(dragbleCard);
    //        if(b == false)
    //        {
    //            ObjectToOrigin(area, obj);
    //        }
    //    }
    //    else
    //    {
    //        ObjectToOrigin(area, obj);
    //    }
    //}


    //private void ObjectLiftedFromReroll(DropArea area, GameObject obj)
    //{

    //}
    //private void ObjectDroppedToReroll(DropArea area, GameObject obj)
    //{
    //    //DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
    //    //CardPower cardPower = obj.GetComponent<CardPower>();
    //    //if (cardPower.dropAreaType == DropAreaType.feild)
    //    //{
    //    //    rerollEvent.Occurred(obj);
    //    //    dragbleCard.isDestory = true;

    //    //    GameManager.Instance.RerollScore();
    //    //}
    //    //else
    //    //{
    //    //    ObjectToOrigin(area, obj);
    //    //}
    //}

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
        Debug.Log("hover drop");

        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        //Debug.Log(dragbleCard.cardPower.cardType);

        obj.transform.SetParent(dragbleCard.originDropArea.rectTrm, true);
        dragbleCard.SetDroppedArea(dragbleCard.originDropArea);

        dragbleCard.cardPower.SetHandle();

        CardPower cardPower = dragbleCard.GetComponent<CardPower>();
        cardPower.OnHandle();

        if(cardPower is BasicCard)
        {
            BasicCard basicCard = cardPower as BasicCard;

            // 다시 돌려 받기
            basicCard.GetApplySpecial();

            // 카드 원상복귀
            basicCard.InitData_Feild(basicCard.originBasicType, basicCard.originValue);
        }

        GameManager.Instance.handleController.cardSorting.AlignCards();
    }
}