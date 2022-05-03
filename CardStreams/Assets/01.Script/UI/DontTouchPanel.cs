using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontTouchPanel : MonoBehaviour
{
    public GameObject raycastTarget;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        raycastTarget.SetActive(true);
    }

    public void Hide()
    {
        raycastTarget.SetActive(false);
    }
}
