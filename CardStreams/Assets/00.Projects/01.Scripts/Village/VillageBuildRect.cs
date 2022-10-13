using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildRect : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Vector2Int rectPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        DropInputManager.Instance.TargetingBuildRect(transform.position);
        VillageShopItem.buildPos = rectPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
    }
}