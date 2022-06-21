using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DontRaycastTarget : MonoBehaviour
{
    public static List<DontRaycastTarget> dontRaycastTargetList;

    public Image dontRayImage;

    public void Awake()
    {
        dontRaycastTargetList = dontRaycastTargetList ?? new List<DontRaycastTarget>();
        dontRaycastTargetList.Add(this);
    }

    public static void SetRaycastTarget(bool enable)
    {
        foreach (var image in dontRaycastTargetList)
        {
            image.dontRayImage.raycastTarget = enable;
        }
    }
}
