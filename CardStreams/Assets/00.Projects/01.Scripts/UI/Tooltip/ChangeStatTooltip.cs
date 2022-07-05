using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ChangeStatTooltip : MonoBehaviour
{
    public static ChangeStatTooltip Instance;

    public GameObject statTextPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(int value, bool isUp, Vector3 pos)
    {
        GameObject statTextObj = Instantiate(statTextPrefab, transform);

        RectTransform statTextRect = statTextObj.GetComponent<RectTransform>();
        TextMeshProUGUI statText = statTextObj.GetComponent<TextMeshProUGUI>();

        statTextRect.anchoredPosition = pos;

        statText.text = value.ToString();
        statText.color = isUp ? Color.blue : Color.red;

        statTextRect.DOAnchorPosY(statTextRect.anchoredPosition.y + 50, 1f);

        Destroy(statTextObj, 1f);
    }
}
