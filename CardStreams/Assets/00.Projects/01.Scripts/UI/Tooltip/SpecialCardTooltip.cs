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

    public void Show(string nameStr, List<CardType> targetTypeList, string tooltipStr, Sprite sprite, Vector3 pos, TooltipTimer tooltipTimer = null)
    {
        // 타이머 세팅
        this.tooltipTimer = tooltipTimer;

        nameText.text = nameStr;

        string str = string.Empty;
        if(targetTypeList != null)
        {
            int index = 0;

            //foreach (CardType targetType in targetTypeList)
            //{
            //    if(index != 0)
            //    {
            //        str += " ";
            //    }

            //    switch (targetType)
            //    {
            //        case CardType.Sword:
            //            str += "<sprite=0>";
            //            break;
            //        case CardType.Sheild:
            //            str += "<sprite=1>";
            //            break;
            //        case CardType.Potion:
            //            str += "<sprite=5>";
            //            break;
            //        case CardType.Monster:
            //            str += "<sprite=3>";
            //            break;
            //        default:
            //            str += "<sprite=2>";
            //            break;
            //    }

            //    index++;
            //}
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
