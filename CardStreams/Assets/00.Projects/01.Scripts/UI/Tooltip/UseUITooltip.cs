using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseUITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string msg;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        UITooltip.Instance.Show(msg);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        UITooltip.Instance.Hide();
    }
}
