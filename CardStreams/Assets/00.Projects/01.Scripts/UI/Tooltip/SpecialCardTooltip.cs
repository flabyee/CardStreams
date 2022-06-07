using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialCardTooltip : MonoBehaviour
{
    public static SpecialCardTooltip Instance { get; private set; }

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

    public void Show(string nameStr, List<CardType> targetTypeList, List<BasicType> targetBasicList, string tooltipStr, Sprite sprite, Vector3 pos, TooltipTimer tooltipTimer = null)
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
                if (index++ != 0)
                    str += " ";

                switch (targetType)
                {
                    // NUll은 player에게 사용하는 것만 존재한다고 가정
                    case CardType.NULL:
                        str += "<sprite=2>";
                        break;
                    case CardType.Basic:
                        foreach (BasicType basicType in targetBasicList)
                        {
                            if (index++ != 0)
                                str += " ";

                            switch (basicType)
                            {
                                case BasicType.Sword:
                                    str += "<sprite=0>";
                                    break;
                                case BasicType.Sheild:
                                    str += "<sprite=1>";
                                    break;
                                case BasicType.Potion:
                                    str += "<sprite=5>";
                                    break;
                                case BasicType.Monster:
                                    str += "<sprite=3>";
                                    break;
                            }
                        }
                        break;
                    case CardType.Build:
                        str += "<sprite=4>";
                        break;
                    default:
                        break;
                }


            }
            targetText.text = str;
        }
        else
        {
            targetText.text = "";
        }


        infoText.text = tooltipStr;
        backgroundImage.sprite = sprite;

        //pos.x += 50;
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
