using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpecialCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public SpecialCardSO specialCardSO;

    [HideInInspector] public ApplyTiming applyTiming;
    [HideInInspector] public List<CardType> targetTypeList;

    [HideInInspector] public string specialCardName;
    [HideInInspector] public Image cardImage;

    public void Init(SpecialCardSO so)
    {
        specialCardSO = so;

        applyTiming = so.applyTiming;
        targetTypeList = so.targetTypeList;

        specialCardName = so.specialCardName;
        cardImage.sprite = specialCardSO.sprite;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        CardTooltip.Instance.Show(specialCardSO.specialCardName, specialCardSO.tooltip, specialCardSO.sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardTooltip.Instance.Hide();
    }
}
