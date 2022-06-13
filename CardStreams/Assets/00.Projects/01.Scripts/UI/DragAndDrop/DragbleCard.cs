using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DragbleCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
    IPointerDownHandler, IPointerUpHandler
{
    public Action<DragbleCard> onMoveStart;
    public Action<DragbleCard> onMoveEnd;
    public Action<DragbleCard> onNothing;

    private RectTransform rectTransform;
    private RectTransform clampRectTransform;

    private Vector3 originalWorldPos;
    private Vector3 originalRectWorldPos;

    private Vector3 minWorldPosition;
    private Vector3 maxWorldPosition;

    public DropArea droppedArea;
    public DropArea prevDropArea;
    public DropArea originDropArea; // 처음 드랍에리어
    public DropArea originOriginDropArea; // 나중에 수정



    public bool canDragAndDrop;
    public bool isDestory;

    public CardPower cardPower;

    private bool isDraging;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        clampRectTransform = rectTransform.root.GetComponent<RectTransform>();

        cardPower = GetComponent<CardPower>();

        isDestory = false;
    }

    public void SetDroppedArea(DropArea dropArea)
    {
        this.droppedArea = dropArea;
    }
    public void SetData_Feild(BasicType basicType, int value)
    {
        canDragAndDrop = true;

        cardPower.SetHandle();

        (cardPower as BasicCard).SetData_Feild(basicType, value);
    }

    public void SetData_SpecialCard()
    {
        canDragAndDrop = false;

        cardPower.SetHandle();

        cardPower.SetData_SpecialCard();
    }

    public void SetData_Build()
    {
        canDragAndDrop = false;

        cardPower.SetHandle();

        cardPower.SetData_Build();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalRectWorldPos = rectTransform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform,
            eventData.position, eventData.pressEventCamera, out originalWorldPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        if(canDragAndDrop == false)
        {
            return;
        }
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        isDraging = true;

        if (onMoveStart != null) onMoveStart(this);

        if (droppedArea != null) droppedArea.TriggerOnLift(this);
        prevDropArea = droppedArea;
        droppedArea = null;

        DropArea.SetDropArea(true, cardPower.cardType);

        // 드래그 시작할 때 설정?
        Rect clamp = new Rect(Vector2.zero, clampRectTransform.rect.size);
        Vector3 minPosition = clamp.min - rectTransform.rect.min;
        Vector3 maxPosition = clamp.max - rectTransform.rect.max;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform, minPosition,
            eventData.pressEventCamera, out minWorldPosition);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform, maxPosition,
            eventData.pressEventCamera, out maxWorldPosition);


        //Debug.Log(minWorldPosition + "/" + maxWorldPosition);


        if(cardPower.cardType == CardType.Build)
        {
            BuildCard build = GetComponent<BuildCard>();

            BuildAreaTooltip.Instance.ShowFollow(transform, build.GetAccessPointList());
        }

        rectTransform.sizeDelta = new Vector2(75, 75);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraging == false)
            return;

        // 드래그 시작할 때 설정한 값을 이용해서 이동
        Vector3 worldPointerPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(clampRectTransform, eventData.position,
            eventData.pressEventCamera, out worldPointerPosition))
        {
            Vector3 offsetToOriginal = worldPointerPosition - originalWorldPos;
            rectTransform.position = originalRectWorldPos + offsetToOriginal;
        }

        Vector3 worldPos = rectTransform.position;
        worldPos.x = Mathf.Clamp(rectTransform.position.x, minWorldPosition.x, maxWorldPosition.x);
        worldPos.y = Mathf.Clamp(rectTransform.position.y, minWorldPosition.y, maxWorldPosition.y);
        rectTransform.position = worldPos;


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraging == false)
            return;


        isDraging = false;

        DropArea.SetDropArea(false, cardPower.cardType);
        if (onMoveEnd != null) onMoveEnd(this);

        bool noEvent = true;
        foreach (var go in eventData.hovered)
        {
            var dropArea = go.GetComponent<DropArea>();
            if (dropArea != null)
            {
                noEvent = false;
                break;
            }
        }

        if (noEvent)
        {
            if (onNothing != null) onNothing(this);
        }


        gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);

        if(isDestory == true)
        {
            ActiveFalse();
        }

        if (cardPower.cardType == CardType.Build)
        {
            BuildAreaTooltip.Instance.HideFollow();
        }
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    //if(eventData.button == PointerEventData.InputButton.Right && isHandle == true)
    //    //{
    //    //    switch(cardPower.cardType)
    //    //    {
    //    //        case CardType.Sword:
    //    //        case CardType.Sheild:
    //    //        case CardType.Potion:
    //    //        case CardType.Monster:
    //    //            GameManager.Instance.DropField(this);
    //    //            break;
    //    //        case CardType.Special:
    //    //            GameManager.Instance.DropQuickSlot(this);
    //    //            break;
    //    //    }
    //    //}
    //}



    public void ActiveFalse()
    {
        transform.SetParent(ConstManager.Instance.tempTrm, true);

        gameObject.SetActive(false);
    } 
}
