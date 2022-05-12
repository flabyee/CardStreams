using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildTooltip : MonoBehaviour
{
    public static BuildTooltip Instance { get; private set; }

    public TextMeshProUGUI nameText;
    public RectTransform areaParentTrm;
    public TextMeshProUGUI infoText;
    public Image backgroundImage;

    public GameObject areaTooltipPrefab;
    private Image[,] areaImageArr = new Image[5, 5];

    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        Instance = this;

        Hide();

        for (int y = 2; y >= -2; y--)
        {
            for (int x = 2; x >= -2; x--)
            {
                GameObject obj = Instantiate(areaTooltipPrefab, areaParentTrm);

                Image image = obj.GetComponent<Image>();

                areaImageArr[y + 2, x + 2] = image;
            }
        }
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

    public void Show(string nameStr, List<Vector2> accessPointList, string tooltipStr, Sprite sprite, Vector3 pos, TooltipTimer tooltipTimer = null)
    {
        // Ÿ�̸� ����
        this.tooltipTimer = tooltipTimer;

        nameText.text = nameStr;

        string str = string.Empty;

        for (int y = 2; y >= -2; y--)
        {
            for (int x = 2; x >= -2; x--)
            {
                // point�� �ش�Ǵ� �����̾ƴ϶�� Alpha = 0
                if (!accessPointList.Contains(new Vector2(x, y)))
                {
                    areaImageArr[y + 2, x + 2].color = new Color(0, 0, 0, 0);
                }
                else
                {
                    areaImageArr[y + 2, x + 2].color = Color.green;
                }

                if(y == 0 && x == 0)
                {
                    areaImageArr[y + 2, x + 2].color = Color.yellow;
                }
            }
        }


        infoText.text = tooltipStr;
        backgroundImage.sprite = sprite;

        pos.x += 30;
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
