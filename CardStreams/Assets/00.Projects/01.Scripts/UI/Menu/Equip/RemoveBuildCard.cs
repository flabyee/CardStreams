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
        // isUse가 true면 꺼지고, false면 켜지고
        removeImage.gameObject.SetActive(!b);
    }
}
