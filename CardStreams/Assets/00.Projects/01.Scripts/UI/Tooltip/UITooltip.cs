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

    }

    private void HandleFollowMouse()
    {
        // ���콺 �������� ����ٴϴ� ui
        // ȭ����� ����, �ϴ��� ������ ����� ��ġ�� �����ȴ�
        Vector2 anchoredPos = Input.mousePosition;// / canvasRectTrm.lossyScale.x;
        rectTrm.anchoredPosition = anchoredPos;

        // x ����
        if(rectTrm.anchoredPosition.x - tooltipWidth / 2 < 10f) // x�� �ʹ� ������(�ּ�10)
        {
            rectTrm.anchoredPosition = new Vector2(tooltipWidth / 2 + 10f, rectTrm.anchoredPosition.y); // x 10���� �÷���
        }
        else if(rectTrm.anchoredPosition.x + tooltipWidth / 2 > Screen.width - 10f) // x�� �ʹ� ������(�ִ� : ȭ�����-10)
        {
            rectTrm.anchoredPosition = new Vector2(Screen.width - (tooltipWidth / 2 + 10f), rectTrm.anchoredPosition.y); // x ȭ�����-10���� ������
        }

        // y ����
        if(rectTrm.anchoredPosition.y - tooltipHeight / 2 < 10f) // y�� �ʹ� ������(�ּ�10)
        {
            rectTrm.anchoredPosition = new Vector2(rectTrm.anchoredPosition.x, tooltipHeight / 2 + 10f); // y 10���� �÷���
        }
        else if(rectTrm.anchoredPosition.y + tooltipHeight / 2 > Screen.height - 10f) // y�� �ʹ� ������(�ִ� : ȭ�����-10)
        {
            rectTrm.anchoredPosition = new Vector2(rectTrm.anchoredPosition.x, Screen.height - (tooltipHeight / 2 + 10f)); // y ȭ�����-10���� ������
        }

        // rectTrm.anchoredPosition = anchoredPos;

        //// ���� ��ġ ����
        //if (anchoredPos.x + backgroundRectTrm.rect.width > canvasRectTrm.rect.width)
        //{
        //    anchoredPos.x = canvasRectTrm.rect.width - backgroundRectTrm.rect.width;
        //}

        //// ���� ��ġ ����
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
        // Ÿ�̸� ����
        // this.tooltipTimer = tooltipTimer;

        // time�� 0 ���϶�� null, �ƴ϶�� new timer
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
