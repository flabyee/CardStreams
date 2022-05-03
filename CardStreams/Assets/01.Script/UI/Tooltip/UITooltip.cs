using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITooltip : MonoBehaviour
{
    public static UITooltip Instance { get; private set; }

    private Text infoText;

    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        Instance = this;

        infoText = transform.Find("InfoText").GetComponent<Text>();

        Hide();
    }

    private void Update()
    {
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

    //private void HandleFollowMouse()
    //{
    //    // 마우스 포지션을 따라다니는 ui
    //    // 화면상의 우측, 하단을 지나면 배경이 위치가 조정된다
    //    Vector2 anchoredPos = Input.mousePosition / canvasRectTrm.lossyScale.x;

    //    // 가로 위치 보정
    //    if (anchoredPos.x + backgroundRectTrm.rect.width > canvasRectTrm.rect.width)
    //    {
    //        anchoredPos.x = canvasRectTrm.rect.width - backgroundRectTrm.rect.width;
    //    }

    //    // 세로 위치 보정
    //    if (anchoredPos.y + backgroundRectTrm.rect.height > canvasRectTrm.rect.height)
    //    {
    //        anchoredPos.y = canvasRectTrm.rect.height - backgroundRectTrm.rect.height;
    //    }

    //    rectTrm.anchoredPosition = anchoredPos;
    //}

    public void Show(string tooltipText, Vector3 pos, TooltipTimer tooltipTimer = null)
    {
        // 타이머 세팅
        this.tooltipTimer = tooltipTimer;

        infoText.text = tooltipText;

        transform.position = pos;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
