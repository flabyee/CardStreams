using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StatPanel : Panel
{
    [Header("�гκ� �ٸ� ������")]
    [SerializeField] IntValue statIntValue;
    private int prevStat = 0;

    [SerializeField] bool isShowMax;

    [Header("�г� �Ʒ� �̹����� �ؽ�Ʈ")]
    [SerializeField] TextMeshProUGUI valueText;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }


    public void StatUIChange() // Called by EventSO from gameObject | ���ӿ�����Ʈ�� ����ִ� EventSO���� ȣ���
    {
        if(prevStat != statIntValue.RuntimeValue)
        {
            int changedValue = statIntValue.RuntimeValue - prevStat;

            Vector3 pos = mainCam.WorldToScreenPoint(transform.position);
            // ������ ���
            if (prevStat < statIntValue.RuntimeValue)
            {
                ChangeStatTooltip.Instance.Show(changedValue, true, pos);
            }
            // ������ ���
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
