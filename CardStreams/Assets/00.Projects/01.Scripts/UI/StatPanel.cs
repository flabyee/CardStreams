using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StatPanel : MonoBehaviour
{
    [Header("패널별 다른 설정값")]
    [SerializeField] bool isRuntimeMaxValueExist = false;
    [SerializeField] IntValue statIntValue; // 칩의 종류(스텟종류)

    [Header("패널 아래 이미지와 텍스트")]
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Image[] runtimeStatImages; // 게임할때 뜨는 칩 개수, 일단 20개만 넣음
    [SerializeField] Sprite[] statUnitSprites; // 1 | 10 | 50 | 100 | 500 | 1000 단위, 지금은 1이랑 10만 넣음

    [SerializeField] bool isShowMax;

    public void StatUIChange() // Called by EventSO from gameObject | 게임오브젝트에 들어있는 EventSO에서 호출됨
    {
        int statAmount = statIntValue.RuntimeValue;

        int count = 0;

        valueText.text = statIntValue.RuntimeValue.ToString();
        if (isShowMax)
        {
            valueText.text += $"/{statIntValue.RuntimeMaxValue}";
        }
        // 지금은 수동으로하는데 나중에 함수빼서 처리해야 /100 /10 /1 에바참치

        return;

        // 여기아래 = 옛날옛적 아이콘으로 1 10 100 수치표기했을시절

        //int divide100 = statAmount / 100;

        //for (int i = 0; i < divide100; i++)
        //{
        //    runtimeStatImages[i].DOFade(1, 0.2f);
        //    runtimeStatImages[i].sprite = statUnitSprites[2]; // 10 이미지로 교체

        //    count++;
        //} // count 1

        //statAmount -= divide100 * 100; // 3

        //int divide10 = statAmount / 10;
        //for (int i = 0; i < divide10; i++)
        //{
        //    runtimeStatImages[i].DOFade(1, 0.2f);
        //    runtimeStatImages[i].sprite = statUnitSprites[1]; // 10 이미지로 교체

        //    count++;
        //} // count 1

        //statAmount -= divide10 * 10; // 3

        //int divide1 = statAmount; // 3
        //for (int i = count; i < divide1 + divide10; i++) // 1 < 3
        //{
        //    runtimeStatImages[i].DOFade(1, 0.2f);
        //    runtimeStatImages[i].sprite = statUnitSprites[0]; // 1 이미지로 교체

        //    count++;
        //}

        //for (int i = count; i < runtimeStatImages.Length; i++)
        //{
        //    int avoidClosure = i;
        //    runtimeStatImages[avoidClosure].DOFade(0, 0.2f);

        //    count++;
        //}
    }
}
