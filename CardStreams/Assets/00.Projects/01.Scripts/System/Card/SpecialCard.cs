using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class SpecialCard : CardPower, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public int id;
    [HideInInspector] public ApplyTiming applyTiming;
    [HideInInspector] public List<CardType> targetTypeList;
    [HideInInspector] public List<BasicType> targetBasicList;

    [HideInInspector] public string specialCardName;
    [HideInInspector] public string tooltip;
    [HideInInspector] public Sprite sprite;

    [Header("UI")]
    public TextMeshProUGUI tooltipText;
    

    public Action<Player, Field> OnAccessSpecialCard;
    public Action<BuildCard> OnAccessBuildCard;

    public void Init(SpecialCardSO so)
    {
        id = so.id;

        applyTiming = so.applyTiming;
        targetTypeList = so.targetTypeList;
        targetBasicList = so.targetBasicList;

        specialCardName = so.specialCardName;
        tooltip = so.tooltip;
        sprite = so.sprite;

        faceImage.sprite = so.sprite;
        fieldImage.sprite = so.sprite;
        nameText.text = so.specialCardName;
        tooltipText.text = so.tooltip;

        OnAccessSpecialCard = so.AccessSpecialCard;
        OnAccessBuildCard = so.AccessBuildCard;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        // SpecialCardTooltip.Instance.Show(specialCardName, targetTypeList, targetBasicList, tooltip, sprite, transform.position);
        HandleCardTooltip.Instance.ShowSpecial(transform.position + transform.up * 0.5f, sprite, specialCardName, tooltip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandleCardTooltip.Instance.Hide();
        // SpecialCardTooltip.Instance.Hide();
    }
}
