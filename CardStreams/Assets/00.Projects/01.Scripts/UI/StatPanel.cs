using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StatPanel : Panel
{
    [Header("패널별 다른 설정값")]
    [SerializeField] IntValue statIntValue;
    private int prevStat = 0;

    [SerializeField] bool isShowMax;

    [Header("패널 아래 이미지와 텍스트")]
    [SerializeField] TextMeshProUGUI valueText;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }


    public void StatUIChange() // Called by EventSO from gameObject | 게임오브젝트에 들어있는 EventSO에서 호출됨
    {
        if(prevStat != statIntValue.RuntimeValue)
        {
            int changedValue = statIntValue.RuntimeValue - prevStat;

            Vector3 pos = mainCam.WorldToScreenPoint(transform.position);
            // 증가한 경우
            if (prevStat < statIntValue.RuntimeValue)
            {
                ChangeStatTooltip.Instance.Show(changedValue, true, pos);
            }
            // 감소한 경우
            else
            {
                ChangeStatTooltip.Instance.Show(changedValue, false, pos);
            }
        }


        prevStat = statIntValue.RuntimeValue;

        valueText.text = statIntValue.RuntimeValue.ToString();
        if (isShowMax)
        {
            valueText.text += $"/{statIntValue.RuntimeMaxValue}";
        }
    }
}
