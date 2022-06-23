using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseUITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string msg;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UITooltip.Instance.Show(msg);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UITooltip.Instance.Hide();
    }
}
