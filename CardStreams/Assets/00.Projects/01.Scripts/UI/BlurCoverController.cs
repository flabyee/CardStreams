using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurCoverController : MonoBehaviour
{
    public GameObject blurObj;

    public void SetActive(bool b)
    {
        blurObj.SetActive(b);
    }
}
