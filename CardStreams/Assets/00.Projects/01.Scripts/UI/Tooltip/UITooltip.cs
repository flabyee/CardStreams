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
        // ���� Ÿ�̸� ó��
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
    //    // ���콺 �������� ����ٴϴ� ui
    //    // ȭ����� ����, �ϴ��� ������ ����� ��ġ�� �����ȴ�
    //    Vector2 anchoredPos = Input.mousePosition / canvasRectTrm.lossyScale.x;

    //    // ���� ��ġ ����
    //    if (anchoredPos.x + backgroundRectTrm.rect.width > canvasRectTrm.rect.width)
    //    {
    //        anchoredPos.x = canvasRectTrm.rect.width - backgroundRectTrm.rect.width;
    //    }

    //    // ���� ��ġ ����
    //    if (anchoredPos.y + backgroundRectTrm.rect.height > canvasRectTrm.rect.height)
    //    {
    //        anchoredPos.y = canvasRectTrm.rect.height - backgroundRectTrm.rect.height;
    //    }

    //    rectTrm.anchoredPosition = anchoredPos;
    //}

    public void Show(string tooltipText, Vector3 pos, TooltipTimer tooltipTimer = null)
    {
        // Ÿ�̸� ����
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
