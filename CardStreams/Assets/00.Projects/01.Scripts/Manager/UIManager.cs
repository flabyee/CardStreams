using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    Clear,
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("이미 존재함");
        }
    }

    [Header("Panels")]
    public GameObject clearPanel;
    public PlayerBuffPanel playerBuffPanel;

    public void Show(PanelType panelType)
    {
        switch(panelType)
        {
            case PanelType.Clear:
                clearPanel.SetActive(true);
                break;
        }
    }
    public void Hide(PanelType panelType)
    {
        switch(panelType)
        {
            case PanelType.Clear:
                clearPanel.SetActive(false);
                break;
        }
    }
}
