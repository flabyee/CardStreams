using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveSpecialCard : MonoBehaviour
{
    public Image cardImage;
    public Image removeImage;
    public Button button;

    public SpecialCardSO specialSO;

    public void Init(SpecialCardSO specialSO, bool isRemove)
    {
        this.specialSO = specialSO;

        cardImage.sprite = specialSO.sprite;

        ActiveRemoveImage(isRemove);
    }

    public void ActiveRemoveImage(bool b)
    {
        removeImage.gameObject.SetActive(!b);
    }
}
