using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VillageShop : MonoBehaviour
{
    private float openPosX = -30;
    private float closePosX = 470;

    private bool isOpen = false;

    private RectTransform _rectTrm;

    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
    }

    public void OpenMenu()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Debug.Log("°¡ÀÚ");
            _rectTrm.DOAnchorPosX(openPosX, 1.0f);
        }
        else
        {
            _rectTrm.DOAnchorPosX(closePosX, 1.0f);
        }
    }
}
