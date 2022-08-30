using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyShortcutManager : MonoBehaviour
{
    public UnityEvent EscPress;
    public UnityEvent SpacePress;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EscPress?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpacePress?.Invoke();
        }
    }

    public void OnClickEsc() // UI�� ����â Ű�� �� ���
    {
        EscPress?.Invoke();
    }

    public void OnClickSpace() // UI�� �Ͻ����� ��ư ���� ���
    {
        SpacePress?.Invoke();
    }
}
