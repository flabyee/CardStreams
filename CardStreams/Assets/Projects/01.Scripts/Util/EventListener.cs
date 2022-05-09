using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject> { }

public class EventListener : MonoBehaviour
{
    public EventSO gEvent;  // 구독중인 것
    public UnityGameObjectEvent responseObj = new UnityGameObjectEvent();

    private void OnEnable()
    {
        gEvent.Register(this);
    }

    private void OnDisable()
    {
        gEvent.UnResister(this);
    }

    // 통보자의 행동을 주시하고 있던 리스너"들"은 그 행동이 무엇인지에 따라 적절한 반응을 보여주면 된다
    public void OnEventOccurs(GameObject obj)
    {
        responseObj.Invoke(obj);
    }
}
