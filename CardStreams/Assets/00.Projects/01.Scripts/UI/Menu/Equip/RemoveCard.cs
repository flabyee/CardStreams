using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveCard : MonoBehaviour
{
    public Image cardImage;
    public Image removeImage;
    public Button button;

    public void ActiveRemoveImage(bool b)
    {
        // isUse가 true면 꺼지고, false면 켜지고
        removeImage.gameObject.SetActive(!b);
    }
}
