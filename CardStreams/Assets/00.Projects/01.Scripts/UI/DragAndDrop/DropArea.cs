using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public enum DropAreaType
{
    NULL,
    feild,
    build,
}

public class DropArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public static List<DropArea> dropAreas;

    public delegate void ObjectLiftEvent(DropArea area, GameObject gameObject);
    public event ObjectLiftEvent onLifted;

    public delegate void ObjectDropEvent(DropArea area, GameObject gameObject);
    public event ObjectDropEvent onDropped;

    public delegate void ObjectHoverEnterEvent(DropArea area, GameObject gameObject);
    public event ObjectHoverEnterEvent onHoverEnter;

    public delegate void ObjectHoverExitEvent(DropArea area, GameObject gameObject);
    public event ObjectHoverExitEvent onHoverExit;

    private Image image;

    public Vector2 point;


    [Header("Debug")]
    public Field field;
    public RectTransform rectTrm;
    public DropAreaType dropAreaType;




    public void Awake()
    {
        dropAreas = dropAreas ?? new List<DropArea>();
        dropAreas.Add(this);
        gameObject.SetActive(false);

        image = GetComponent<Image>();
    }

    public void OnEnable()
    {
        //onLifted += ObjectLifted;
        ////onDropped += ObjectDropped;
        onHoverEnter += ObjectHoveredEnter;
        onHoverExit += ObjectHoveredExit;
    }

    public void OnDisable()
    {
        //onLifted -= ObjectLifted;
        //onDropped -= ObjectDropped;
        onHoverEnter -= ObjectHoveredEnter;
        onHoverExit -= ObjectHoveredExit;
    }

    //public void ObjectLifted(DropArea area, GameObject gameObject)
    //{
    //    Debug.Log(this.gameObject.name + " Object Lifted : " + gameObject.name);
    //}
    //public void ObjectDropped(DropArea area, GameObject gameObject)
    //{
    //    Debug.Log(this.gameObject.name + " Object Dropped : " + gameObject.name);
    //}
    public void ObjectHoveredEnter(DropArea area, GameObject gameObject)
    {
        //Debug.Log(this.gameObject.name + " Object Hovered Enter : " + gameObject.name);
    }
    public void ObjectHoveredExit(DropArea area, GameObject gameObject)
    {
        //Debug.Log(this.gameObject.name + " Object Hovered Exit : " + gameObject.name);
    }

    public void TriggerOnLift(DragbleCard item)
    {
        onLifted(this, item.gameObject);
    }
    public void TriggerOnDrop(DragbleCard item)
    {
        item.SetDroppedArea(this);
        onDropped(this, item.gameObject);
    }
    public void TriggerOnHoverEnter(GameObject gameObject)
    {
        onHoverEnter?.Invoke(this, gameObject);
    }
    public void TriggerOnHoverExit(GameObject gameObject)
    {
        onHoverExit?.Invoke(this, gameObject);
    }





    public void OnPointerEnter(PointerEventData eventData)
    {
        var gameObject = eventData.pointerDrag;
        if (gameObject == null) return;

        TriggerOnHoverEnter(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var gameObject = eventData.pointerDrag;
        if (gameObject == null) return;


        TriggerOnHoverExit(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject gameObject = eventData.selectedObject;
        if (gameObject == null) return;

        var draggable = gameObject.GetComponent<DragbleCard>();
        if (draggable == null) return;

        TriggerOnDrop(draggable);
    }

    public static void SetDropArea(bool enable, CardType dragCardType)
    {
        DropAreaType dropAreaType = DropAreaType.NULL;

        switch (dragCardType)
        {
            case CardType.Basic:
                dropAreaType = DropAreaType.feild;
                break;
            case CardType.Special:
                // To Do : target 따라서 다르게 처리하기
                break;
            case CardType.Build:
                dropAreaType = DropAreaType.build;
                break;
        }

        foreach (var area in dropAreas)
        {
            area.gameObject.SetActive(enable);
            if(area.dropAreaType == DropAreaType.NULL)
            {

            }
            // 같은 dropAreaType인지
            else if (area.dropAreaType == dropAreaType)
            {
                // feild라면 feildType이 able인지
                if(area.field != null)
                {
                    if(area.field.fieldState == FieldState.able && area.field.transform.childCount == 0)
                    {
                        area.image.color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        area.image.color = new Color(1, 1, 1, 0);
                    }
                }
                // build라면 이미 설치된게있는지
                else
                {
                    if(area.rectTrm.childCount == 0)
                    {
                        area.image.color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        area.image.color = new Color(1, 1, 1, 0);
                    }
                }
            }
            else
            {
                area.image.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
