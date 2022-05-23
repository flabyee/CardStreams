using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewHeightController : MonoBehaviour
{
    private int originChildCount;
    private RectTransform rectTrm;

    public int height;
    // Start is called before the first frame update

    private void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        originChildCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(originChildCount != transform.childCount)
        {
            originChildCount = transform.childCount;

            int heightCount = Mathf.CeilToInt(transform.childCount / 2f);

            rectTrm.sizeDelta = new Vector2(0, heightCount * height);
        }
    }
}
