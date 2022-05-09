using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;

    private List<DropArea> feildDropAreaList = new List<DropArea>();
    private List<DropArea> buildDropAreaList = new List<DropArea>();

    public DropArea handleDropArea;
    public RectTransform handleTrm;

    public DropArea buildHandleDropArea;
    public RectTransform buildHandleTrm;

    public DropArea shopDropArea;
    public RectTransform shopTrm;

    public DropArea rerollDropArea;
    public RectTransform rerollTrm;

    public DropArea hoverDropArea;
    public RectTransform hoverTrm;



    private void Awake()
    {
        Instance = this;

        

        // rect의 위치를 가져와서 dropArea 위치 설정
        //foreach(DropArea dropArea in DropArea.dropAreas)
        //{
        //    RectTransform dropRect = dropArea.GetComponent<RectTransform>();

        //    Debug.Log(dropArea.rectTrm.gameObject.name + " : " + dropArea.rectTrm.rect.width + " " + dropArea.rectTrm.rect.height);

        //    dropRect.sizeDelta = dropArea.rectTrm.sizeDelta;
        //    dropRect.transform.position = dropArea.rectTrm.transform.position;
        //}
    }

    private void Start()
    {
        

        foreach(DropArea dropArea in DropArea.dropAreas)
        {
            if(dropArea.dropAreaType == DropAreaType.feild)
            {
                feildDropAreaList.Add(dropArea);
            }
            else if(dropArea.dropAreaType == DropAreaType.build)
            {
                buildDropAreaList.Add(dropArea);
            }
        }

        foreach (var item in feildDropAreaList)
        {
            item.onLifted += ObjectLiftedFromFeild;
            item.onDropped += ObjectDroppedToFeild;
        }

        foreach (var item in buildDropAreaList)
        {
            item.onLifted += ObjectLiftedFromBuild;
            item.onDropped += ObjectDroppedToBuild;
        }

        handleDropArea.onLifted += ObjectLiftedFromHandle;
        handleDropArea.onDropped += ObjectDroppedToHandle;

        buildHandleDropArea.onLifted += ObjectLiftedFromBuildHandle;
        buildHandleDropArea.onDropped += ObjectDroppedToBuildHandle;

        shopDropArea.onLifted += ObjectLiftedFromShop;
        shopDropArea.onDropped += ObjectDroppedToShop;

        rerollDropArea.onLifted += ObjectLiftedFromReroll;
        rerollDropArea.onDropped += ObjectDroppedToReroll;

        hoverDropArea.onLifted += ObjectLiftedFromHover;
        hoverDropArea.onDropped += ObjectDroppedToHover;
    }

    private void ObjectLiftedFromFeild(DropArea area, GameObject obj)
    {
        obj.transform.SetParent(hoverTrm, true);

        area.field.ResetData();

        // fieldType 설정
        area.field.fieldType = FieldType.able;
    }
    private void ObjectDroppedToFeild(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        // field에 놓는게 아니라면(예시 : 건물) 다시 원위치
        if (cardPower.dropAreaType != DropAreaType.feild)  
        {
            ObjectToOrigin(area, obj);
        }

        // 카드가 일반 카드라면
        if(cardPower.cardType != CardType.Special && cardPower.cardType != CardType.Build)
        {
            // 1. 설치가능한곳 인지 3, To Do : area.field.fieldType == FieldType.randomMob 이거는 왜하냐?
            if (area.field.fieldType == FieldType.able || area.field.fieldType == FieldType.randomMob)
            {
                // 2.이미 뭐가 배치되어있는지 확인, 
                if (area.field.cardType == CardType.NULL)
                {
                    // 부모 설정
                    obj.transform.SetParent(area.rectTrm, true);

                    // 정보 설정
                    area.field.cardPower = cardPower;
                    area.field.SetData(area.field.cardPower);

                    // fieldType 설정
                    area.field.fieldType = FieldType.not;
                }
                else
                {
                    // 놓으려고 한곳의 있던 카드의 드랍에이어 얻기
                    DropArea myDropArea = dragbleCard.droppedArea;  // 내가 있던 dropArea
                    DropArea changeDropArea = area;                 // 드랍한 곳의 dropArea
                    DragbleCard otherDragbleCard = changeDropArea.transform.GetChild(0).GetComponent<DragbleCard>();
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
                foreach(CardType targetType in specialCard.targetTypeList)
                {
                    if(area.field.cardType == targetType)
                    {
                        switch (specialCard.applyTiming)
                        {
                            case ApplyTiming.Now:
                                specialCard.OnAccessSpecialCard(GameManager.Instance.player, area.field);
                                break;
                            case ApplyTiming.MoveStart:
                                //area.feild.accessBuildToCardAfterMoveStart += specialCard.AccessSpecialCard;
                                break;
                            case ApplyTiming.OnFeild:
                                //area.feild.accessBeforeOnField += specialCard.OnAccessSpecialCard;
                                break;
                        }

                        // 맞는게 있다면 효과적용하고 스페셜 카드 삭제

                        dragbleCard.isDestory = true;

                        return;
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

        // 아무것도없고 건물카드라면
        if (area.rectTrm.childCount == 0 && cardPower.dropAreaType == DropAreaType.build)
        {
            // 부모 설정(위치 설정)
            obj.transform.SetParent(area.rectTrm, true);

            // 못움직이게 설정
            dragbleCard.canDragAndDrop = false;

            // 건물 효과 적용
            Build build = obj.GetComponent<Build>();
            build.BuildDrop();

        }
        // 뭐가 있거나 건물카드가 아니라면
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
        if(cardPower.cardType != CardType.Monster && cardPower.cardType != CardType.Coin)
        {
            GameManager.Instance.AddScore(2);
            //dragbleCard.isDestory = true;

            cardPower.value = 0;
            cardPower.ApplyUI();

            ObjectToOrigin(area, obj);
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
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();
        if(cardPower.dropAreaType == DropAreaType.feild)
        {
            HandleManager.Instance.CardRerollAdd(cardPower.cardType, cardPower.value, cardPower.dropAreaType);
            dragbleCard.isDestory = true;

            HandleManager.Instance.DrawCard(true);
            GameManager.Instance.RerollScore();
        }
        else
        {
            ObjectToOrigin(area, obj);
        }
    }


    private void ObjectLiftedFromHover(DropArea area, GameObject obj)
    {

    }
    private void ObjectDroppedToHover(DropArea area, GameObject obj)
    {
        Debug.Log("hover drop");
        ObjectToOrigin(area, obj);
    }


    private void ObjectToOrigin(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        Debug.Log(dragbleCard.cardPower.cardType);

        obj.transform.SetParent(dragbleCard.originDropArea.rectTrm, true);
        dragbleCard.SetDroppedArea(dragbleCard.originDropArea);
    }
}