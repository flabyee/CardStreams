using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    public EventSO gEvent;  // �������� ��
    public UnityEvent responseObj = new UnityEvent();

    private void OnEnable()
    {
        gEvent.Register(this);
    }

    private void OnDisable()
    {
        gEvent.UnResister(this);
    }

    // �뺸���� �ൿ�� �ֽ��ϰ� �ִ� ������"��"�� �� �ൿ�� ���������� ���� ������ ������ �����ָ� �ȴ�
    public void OnEventOccurs()
    {
        responseObj.Invoke();
    }
}
