using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class CardPower : MonoBehaviour
{
    public Image faceImage;
    public Image backImage;

    private RectTransform rectTransform;
    public GameObject handleObj;
    public GameObject fieldObj;

    public bool isHandle;
    public bool isField;

    [Header("Debug")]
    public CardType cardType;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void InitData_Feild(BasicType basicType, int value)
    {
        this.cardType = CardType.Basic;
    }

    public void InitData_SpecialCard()
    {
        this.cardType = CardType.Special;
    }

    public void InitData_Build()
    {
        this.cardType = CardType.Build;
    }

    public void SetHandle()
    {
        isHandle = true;
        isField = false;
    }

    public void SetField()
    {
        isHandle = false;
        isField = true;
    }

    public virtual void OnHandle()
    {
        rectTransform.sizeDelta = new Vector2(75, 100);

        handleObj.SetActive(true);
        fieldObj.SetActive(false);
    }
    public virtual void OnField()
    {
        rectTransform.sizeDelta = new Vector2(65, 65);

        fieldObj.SetActive(true);
        handleObj.SetActive(false);
    }
    public virtual void OnHover()
    {
        rectTransform.sizeDelta = new Vector2(100, 100);

        fieldObj.SetActive(true);
        handleObj.SetActive(false);
    }
}
