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

        

        // rect�� ��ġ�� �����ͼ� dropArea ��ġ ����
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

        // fieldType ����
        area.field.fieldType = FieldType.able;
    }
    private void ObjectDroppedToFeild(DropArea area, GameObject obj)
    {
        DragbleCard dragbleCard = obj.GetComponent<DragbleCard>();
        CardPower cardPower = obj.GetComponent<CardPower>();

        //  && area.dropAreaType == DropAreaType.feild�� ���� �ʿ䰡 ����. �ֳ��ϸ� ���ʿ� onDropped += �� �ٸ��� ����� ������
        if (cardPower.dropAreaType != DropAreaType.feild)  
        {
            ObjectToOrigin(area, obj);
        }

        if(cardPower.cardType != CardType.Special && cardPower.cardType != CardType.Build)
        {
            if (area.field.cardType == CardType.NULL && (area.field.fieldType == FieldType.able || area.field.fieldType == FieldType.randomMob))
            {
                // �θ� ����
                obj.transform.SetParent(area.rectTrm, true);

                // ���̻� �������̰� ����
                //dragbleCard.canDragAndDrop = false;

                // ���� ����
                area.field.cardPower = cardPower;
                area.field.SetData(area.field.cardPower);

                // fieldType ����
                area.field.fieldType = FieldType.not;
            }
            else
            {
                ObjectToOrigin(area, obj);
            }
        }
        else
        {
            if (cardPower.cardType == CardType.Special)
            {
                SpecialCard specialCard = obj.GetComponent<SpecialCard>();

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

                        dragbleCard.isDestory = true;

                        return;
                    }
                }

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

        if (area.rectTrm.childCount == 0 && cardPower.dropAreaType == DropAreaType.build)
        {
            obj.transform.SetParent(area.rectTrm, true);

            dragbleCard.canDragAndDrop = false;

            Build build = obj.GetComponent<Build>();
            build.BuildDrop();

        }
        else
        {
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