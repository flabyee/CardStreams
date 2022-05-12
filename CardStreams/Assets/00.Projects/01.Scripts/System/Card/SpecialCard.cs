using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SpecialCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public ApplyTiming applyTiming;
    [HideInInspector] public List<CardType> targetTypeList;

    [HideInInspector] public string specialCardName;
    [HideInInspector] public string tooltip;
    [HideInInspector] public Sprite sprite;

    [HideInInspector] public Image cardImage;

    public Action<Player, Field> OnAccessSpecialCard;

    public void Init(SpecialCardSO so)
    {
        applyTiming = so.applyTiming;
        targetTypeList = so.targetTypeList;

        specialCardName = so.specialCardName;
        tooltip = so.tooltip;
        sprite = so.sprite;

        cardImage.sprite = so.sprite;

        OnAccessSpecialCard = so.AccessSpecialCard;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        SpecialCardTooltip.Instance.Show(specialCardName, targetTypeList, tooltip, sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpecialCardTooltip.Instance.Hide();
    }
}
