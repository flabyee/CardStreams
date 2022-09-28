using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemoveCard : MonoBehaviour
{
    public Image cardImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public Image removeImage;
    public Button button;

    public void ActiveRemoveImage(bool b)
    {
        // isUse�� true�� ������, false�� ������
        removeImage.gameObject.SetActive(!b);
    }
}
