using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Menu<T> : Menu where T : Menu<T>
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public static void Open()
    {
        if (MenuManager.Instance != null && Instance != null)
        {
            MenuManager.Instance.OpenMenu(Instance);
            //Instance.OnOpen();
        }
    }

    // menu를 열때 id 전달할수있게
    public static void Open(int index)
    {
        if (MenuManager.Instance != null && Instance != null)
        {
            MenuManager.Instance.OpenMenu(Instance, index);
            //Instance.OnOpen();
        }
    }
}

public abstract class Menu : MonoBehaviour
{
    public Action onBackPressed;
    protected int isReceiveID = -1; // 메뉴를 열때(OpenMenu) id를 보내주었는가? 아니라면 -1

    public virtual void OnOpen()
    {
    }


    public virtual void OnBackPressed()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.CloseMenu();
        }
    }
}
