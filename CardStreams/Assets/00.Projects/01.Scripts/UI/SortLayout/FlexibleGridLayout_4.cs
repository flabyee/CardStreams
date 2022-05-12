using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �ڽ��� ������ 4 �̸��̸� 4���� ����ϴ�
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

        // �⺻�϶��� rows�� columns �ڵ� ����
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;

            float sqrRt = Mathf.Sqrt(childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        // fixed��� ��� rows�� columns ���߿� �ϳ��� ���� ����
        // �������� �ϳ��� ���� ������ ������ ������ ����
        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(childCount / (float)rows);
        }

        // FixedRowsAndColumns��� �Ѵ� ���� �������ش�
        if (fitType == FitType.FixedRowsAndColumns)
        {
            // rows�� columns �Ѵ� �ν����Ϳ��� ����
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
