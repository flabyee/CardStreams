using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagController : MonoBehaviour
{
    private void Awake()
    {
        Hide();
    }

    public void Show()
    {

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnBuild()
    {

    }

    public void OnSpecial()
    {

    }

    public void OnClickExit()
    {

    }
}
