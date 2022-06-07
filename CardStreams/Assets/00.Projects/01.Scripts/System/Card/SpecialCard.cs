using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SpecialCard : CardPower, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public ApplyTiming applyTiming;
    [HideInInspector] public List<CardType> targetTypeList;
    [HideInInspector] public List<BasicType> targetBasicList;

    [HideInInspector] public string specialCardName;
    [HideInInspector] public string tooltip;
    [HideInInspector] public Sprite sprite;

    public Action<Player, Field> OnAccessSpecialCard;
    public Action<BuildCard> OnAccessBuildCard;

    public void Init(SpecialCardSO so)
    {
        applyTiming = so.applyTiming;
        targetTypeList = so.targetTypeList;
        targetBasicList = so.targetBasicList;

        specialCardName = so.specialCardName;
        tooltip = so.tooltip;
        sprite = so.sprite;

        faceImage.sprite = so.sprite;

        OnAccessSpecialCard = so.AccessSpecialCard;
        OnAccessBuildCard = so.AccessBuildCard;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        SpecialCardTooltip.Instance.Show(specialCardName, targetTypeList, targetBasicList, tooltip, sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpecialCardTooltip.Instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isHandle)
        {
            GameManager.Instance.DropQuickSlot(GetComponent<DragbleCard>());
        }
    }
}
