using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyShortcutManager : MonoBehaviour
{
    public UnityEvent EscPress;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EscPress?.Invoke();
        }
    }
}
