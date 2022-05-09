using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardTooltip : MonoBehaviour
{
    public static CardTooltip Instance { get; private set; }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI infoText;
    public Image backgroundImage;

    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        Instance = this;

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

    public void Show(string nameStr, List<CardType> targetTypeList, string tooltipStr, Sprite sprite, Vector3 pos, TooltipTimer tooltipTimer = null)
    {
        // 타이머 세팅
        this.tooltipTimer = tooltipTimer;

        nameText.text = nameStr;

        string str = string.Empty;
        if(targetTypeList != null)
        {
            int index = 0;

            foreach (CardType targetType in targetTypeList)
            {
                if(index != 0)
                {
                    str += ", ";
                }
                str += targetType.ToString();

                index++;
            }
            targetText.text = str;
        }
        else
        {
            targetText.text = "";
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
