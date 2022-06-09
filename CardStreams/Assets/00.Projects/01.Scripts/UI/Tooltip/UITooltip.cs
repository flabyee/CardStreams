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
        Vector2 anchoredPos = (Input.mousePosition);// / canvasRectTrm.lossyScale.x;

        // 가로 위치 보정
        if (anchoredPos.x + backgroundRectTrm.rect.width > canvasRectTrm.rect.width)
        {
            anchoredPos.x = canvasRectTrm.rect.width - backgroundRectTrm.rect.width;
        }

        // 세로 위치 보정
        if (anchoredPos.y + backgroundRectTrm.rect.height > canvasRectTrm.rect.height)
        {
            anchoredPos.y = canvasRectTrm.rect.height - backgroundRectTrm.rect.height;
        }

        rectTrm.anchoredPosition = anchoredPos;
    }

    void SetText(string tooltipText)
    {

        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(7, 7);
        backgroundRectTrm.sizeDelta = textSize + padding;
    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        // 타이머 세팅
        this.tooltipTimer = tooltipTimer;

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
