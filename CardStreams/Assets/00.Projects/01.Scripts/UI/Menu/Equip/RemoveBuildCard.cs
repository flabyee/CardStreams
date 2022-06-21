using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveBuildCard : MonoBehaviour
{
    public Image cardImage;
    public Image removeImage;
    public Button button;

    public BuildSO buildSO;

    public void Init(BuildSO buildSO, bool isRemove)
    {
        this.buildSO = buildSO;

        cardImage.sprite = buildSO.sprite;

        ActiveRemoveImage(isRemove);
    }

    public void ActiveRemoveImage(bool b)
    {
        // isUse�� true�� ������, false�� ������
        removeImage.gameObject.SetActive(!b);
    }
}
