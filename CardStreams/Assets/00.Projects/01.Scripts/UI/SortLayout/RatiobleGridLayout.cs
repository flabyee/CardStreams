using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatiobleGridLayout : LayoutGroup
{
    // 이 스크립트는 잘못건들이면 망합니다
    public enum FitType
    {
        Null,
        Width,
        Height
    }

    public FitType fitType;
    public int count;   // 안전장치

    [Range(0, 1000)]
    public int spacing;

    [Range(1, 10)]
    public int[] ratioValues = null;

    public override void CalculateLayoutInputVertical()
    {
        float parentWidth = rectTransform.rect.width - padding.left - padding.right;
        float parentHeight = rectTransform.rect.height - padding.top - padding.bottom;

        if (ratioValues == null || count != rectTransform.childCount || fitType == FitType.Null)
        {
            //Debug.LogError("설정이 되어있지 않거나 실제 갯수와 다릅니다");

            return;
        }

        if (fitType == FitType.Width)
        {
            float value = 0;  // 1등분에 들어가는 값
            int allRatio = 0;
            for (int i = 0; i < ratioValues.Length; i++)
            {
                allRatio += ratioValues[i];
            }
            value = parentWidth / allRatio;

            if (ratioValues.Length != rectChildren.Count)
            {
                //Debug.LogError("설정 값이 실제 갯수와 다릅니다");

                return;
            }

            float xPos = padding.left;
            float yPos = padding.top;
            for (int i = 0; i < rectChildren.Count; i++)
            {
                var item = rectChildren[i];

                var widht = value * ratioValues[i] - (((float)spacing / (float)count * 2f));

                SetChildAlongAxis(item, 0, xPos, widht);
                SetChildAlongAxis(item, 1, yPos, parentHeight);

                xPos += widht + spacing;
            }
        }
        if (fitType == FitType.Height)
        {
            float value = 0;  // 1등분에 들어가는 값
            int allRatio = 0;
            for (int i = 0; i < ratioValues.Length; i++)
            {
                allRatio += ratioValues[i];
            }
            value = parentHeight / allRatio;

            if (ratioValues.Length != rectChildren.Count)
            {
                //Debug.LogError("설정 값이 실제 갯수와 다릅니다");

                return;
            }

            float xPos = padding.left;
            float yPos = padding.top;
            for (int i = 0; i < rectChildren.Count; i++)
            {
                var item = rectChildren[i];

                var height = value * ratioValues[i] - (((float)spacing / (float)count * 2f));

                SetChildAlongAxis(item, 0, xPos, parentWidth);
                SetChildAlongAxis(item, 1, yPos, height);

                yPos += height + spacing;
            }
        }
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}
