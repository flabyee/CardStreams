using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RemoveBuildCard : RemoveCard, IPointerEnterHandler, IPointerExitHandler
{
    public BuildSO buildSO;

    public void Init(BuildSO buildSO, bool isRemove)
    {
        this.buildSO = buildSO;

        cardImage.sprite = buildSO.sprite;
        nameText.text = buildSO.buildName;
        infoText.text = buildSO.tooltip;

        ActiveRemoveImage(isRemove);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildTooltip.Instance.Show(buildSO.buildName, buildSO.accessPointList, 
            buildSO.tooltip, buildSO.sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuildTooltip.Instance.Hide();
    }
}
