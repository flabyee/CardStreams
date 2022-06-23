using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RemoveSpecialCard : RemoveCard, IPointerEnterHandler, IPointerExitHandler
{
    public SpecialCardSO specialSO;

    public void Init(SpecialCardSO specialSO, bool isRemove)
    {
        this.specialSO = specialSO;

        cardImage.sprite = specialSO.sprite;

        ActiveRemoveImage(isRemove);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        SpecialCardTooltip.Instance.Show(specialSO.specialCardName, specialSO.targetTypeList, 
            specialSO.targetBasicList, specialSO.tooltip, specialSO.sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpecialCardTooltip.Instance.Hide();
    }
}
