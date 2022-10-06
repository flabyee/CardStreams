using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : Panel
{
    public Image[] buildingRangeImages;
    public TextMeshProUGUI buildTooltipText;
    public Image buildIconImage;

    private RectTransform _rectTrm;

    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
    }

    public void PanelUpdate(BuildSO so)
    {
        List<Vector2> s = so.accessPointList;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                Vector2 dir = new Vector2(x - 1, y - 1); // 0 1 2���� -1 0 1�� �ٲٱ�

                if(s.Contains(dir)) // ������ �ִ�!
                {
                    buildingRangeImages[y * 3 + x].color = Color.black;
                }
                else // ����
                {
                    buildingRangeImages[y * 3 + x].color = Color.gray;
                }
            }
        }
    }

    public void MovePanelX(float anchorX)
    {
        _rectTrm.anchoredPosition = new Vector2(anchorX, _rectTrm.anchoredPosition.y);
    }
}
