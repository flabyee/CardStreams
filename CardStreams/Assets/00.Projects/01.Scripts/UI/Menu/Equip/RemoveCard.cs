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
        // isUse�� true�� ������, false�� ������
        removeImage.gameObject.SetActive(!b);
    }
}
