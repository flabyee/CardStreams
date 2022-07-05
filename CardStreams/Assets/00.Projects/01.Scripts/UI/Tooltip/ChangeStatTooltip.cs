using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeStatTooltip : MonoBehaviour
{
    public static ChangeStatTooltip Instance;

    public TextMeshProUGUI[] valueText;

    private RectTransform rectTrm;

    private void Awake()
    {
        rectTrm = GetComponent<RectTransform>();

        Instance = this;
    }

    public void Show(int value, bool isUp, Vector3 pos, int statType)
    {
        valueText[statType].GetComponent<RectTransform>().anchoredPosition = pos;

        valueText[statType].text = value.ToString();

        valueText[statType].color = isUp ? Color.blue : Color.red;

        valueText[statType].gameObject.SetActive(true);
    }
}
