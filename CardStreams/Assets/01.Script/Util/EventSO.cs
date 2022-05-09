using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new event", menuName = "EventSO")]
public class EventSO : ScriptableObject
{
    private List<EventListener> eventListenerList = new List<EventListener>();  // 나를 구독하는 중인 것들

    // 등록
    public void Register(EventListener listener)
    {
        eventListenerList.Add(listener);
    }

    // 해지
    public void UnResister(EventListener listener)
    {
        eventListenerList.Remove(listener);
    }

    // 발생
    public void Occurred(GameObject obj = null)
    {
        for (int i = 0; i < eventListenerList.Count; i++)
        {
            eventListenerList[i].OnEventOccurs(obj);
        }
    }
}
