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

    public void OnClickEsc() // UI로 설정창 키려 할 경우
    {
        EscPress?.Invoke();
    }

    public void OnClickSpace() // UI로 일시정지 버튼 누를 경우
    {
        SpacePress?.Invoke();
    }
}
