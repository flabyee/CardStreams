using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 자식의 갯수가 4 미만이면 4개로 취급하는
public class FlexibleGridLayout_4 : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns,
        FixedRowsAndColumns,
        RatioWidh,
        RatioHeight
    }

    public FitType fitType;

    public int rows;
    public int columns;

    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;


    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();

        int childCount;
        if (transform.childCount > 3)
            childCount = transform.childCount;
        else
            childCount = 4;

        // 기본일때는 rows랑 columns 자동 설정
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;

            float sqrRt = Mathf.Sqrt(childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        // fixed모드 라면 rows랑 columns 둘중에 하나만 직접 설정
        // 나머지는 하나는 직접 설정한 값으로 나눠서 결정
        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(childCount / (float)rows);
        }

        // FixedRowsAndColumns라면 둘다 직접 설정해준다
        if (fitType == FitType.FixedRowsAndColumns)
        {
            // rows와 columns 둘다 인스팩터에서 설정
        }

        float parenWidth = rectTransform.rect.width;
        float parenHeight = rectTransform.rect.height;

        float cellWidth = parenWidth / (float)columns - ((spacing.x / (float)columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = parenHeight / (float)rows - ((spacing.y / (float)rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < childCount; i++)
        {
            if(i < rectChildren.Count)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
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
