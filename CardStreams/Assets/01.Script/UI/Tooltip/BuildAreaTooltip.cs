using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAreaTooltip : MonoBehaviour
{
    public static BuildAreaTooltip Instance;
    private bool isShow;
     
    private void Awake()
    {
        Hide();
        Instance = this;
    }

    private void Update()
    {
        if(isShow)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            transform.position = pos;
        }
    }

    public void Show()
    {
        //gameObject.SetActive(true);
        isShow = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isShow = false;
    }
}
