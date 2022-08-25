using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITooltip : MonoBehaviour
{
    public static UITooltip Instance { get; private set; }

    [SerializeField]
    private Canvas canvas;
    private RectTransform canvasRectTrm;
    private CanvasScaler scaler;

    private RectTransform rectTrm;

    private TextMeshProUGUI textMeshPro;

    private RectTransform backgroundRectTrm;

    private TooltipTimer tooltipTimer;

    private bool isShow;

    private float tooltipWidth;
    private float tooltipHeight;

    private void Awake()
    {
        Instance = this;

        rectTrm = GetComponent<RectTransform>();
        textMeshPro = this.transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundRectTrm = transform.Find("background").GetComponent<RectTransform>();

        canvasRectTrm = canvas.GetComponent<RectTransform>();
        scaler = canvas.GetComponent<CanvasScaler>();

        Hide();
    }

    private void Update()
    {
        if(isShow)
        {
            HandleFollowMouse();

            // 툴팁 타이머 처리
            if (tooltipTimer != null)
            {
                tooltipTimer.timer -= Time.deltaTime;
                if (tooltipTimer.timer <= 0)
                {
                    Hide();
                }
            }
        }

    }

    private void HandleFollowMouse()
    {
        // 마우스 포지션을 따라다니는 ui
        // 화면상의 우측, 하단을 지나면 배경이 위치가 조정된다
        Vector2 anchoredPos = Input.mousePosition;// / canvasRectTrm.lossyScale.x;
        rectTrm.anchoredPosition = anchoredPos;

        // x 보정
        if(rectTrm.anchoredPosition.x - tooltipWidth / 2 < 10f) // x가 너무 낮으면(최소10)
        {
            rectTrm.anchoredPosition = new Vector2(tooltipWidth / 2 + 10f, rectTrm.anchoredPosition.y); // x 10으로 올려줌
        }
        else if(rectTrm.anchoredPosition.x + tooltipWidth / 2 > Screen.width - 10f) // x가 너무 높으면(최대 : 화면길이-10)
        {
            rectTrm.anchoredPosition = new Vector2(Screen.width - (tooltipWidth / 2 + 10f), rectTrm.anchoredPosition.y); // x 화면길이-10으로 내려줌
        }

        // y 보정
        if(rectTrm.anchoredPosition.y - tooltipHeight / 2 < 10f) // y가 너무 낮으면(최소10)
        {
            rectTrm.anchoredPosition = new Vector2(rectTrm.anchoredPosition.x, tooltipHeight / 2 + 10f); // y 10으로 올려줌
        }
        else if(rectTrm.anchoredPosition.y + tooltipHeight / 2 > Screen.height - 10f) // y가 너무 높으면(최대 : 화면높이-10)
        {
            rectTrm.anchoredPosition = new Vector2(rectTrm.anchoredPosition.x, Screen.height - (tooltipHeight / 2 + 10f)); // y 화면높이-10으로 내려줌
        }

        // rectTrm.anchoredPosition = anchoredPos;

        //// 가로 위치 보정
        //if (anchoredPos.x + backgroundRectTrm.rect.width > canvasRectTrm.rect.width)
        //{
        //    anchoredPos.x = canvasRectTrm.rect.width - backgroundRectTrm.rect.width;
        //}

        //// 세로 위치 보정
        //if (anchoredPos.y + backgroundRectTrm.rect.height > canvasRectTrm.rect.height)
        //{
        //    anchoredPos.y = canvasRectTrm.rect.height - backgroundRectTrm.rect.height;
        //}
        //rectTrm.anchoredPosition = anchoredPos;
    }

    void SetText(string tooltipText)
    {

        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(7, 7);

        Vector2 size = textSize + padding;

        backgroundRectTrm.sizeDelta = size;
        tooltipWidth = size.x;
        tooltipHeight = size.y;
    }

    public void Show(string tooltipText, float time = 0f)
    {
        // 타이머 세팅
        // this.tooltipTimer = tooltipTimer;

        // time이 0 이하라면 null, 아니라면 new timer
        TooltipTimer timer = (time <= 0) ? null : new TooltipTimer(time);
        this.tooltipTimer = timer;

        gameObject.SetActive(true);
        SetText(tooltipText);

        HandleFollowMouse();

        isShow = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        isShow = false;
    }

    public class TooltipTimer
    {
        public float timer;

        public TooltipTimer(float time)
        {
            timer = time;
        }
    }
}
