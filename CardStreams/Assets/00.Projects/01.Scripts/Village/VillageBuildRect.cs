using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VillageBuildRect : MonoBehaviour, IPointerDownHandler
{
    public Vector2Int rectPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        
        DropInputManager.Instance.TargetingBuildRect(transform.position);
        VillageShopItem.buildPos = rectPos;
        Debug.Log(rectPos);
    }
}