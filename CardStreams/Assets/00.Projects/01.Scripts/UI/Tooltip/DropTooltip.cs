using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropTooltip : MonoBehaviour
{


    /// <summary>
    /// 다음에 오면 Show Hide active true false 말고 color a로 ㄱㄱㄱ
    /// </summary>
    public static DropTooltip Instance;

    public GameObject dropTooltipObj;
    public RectTransform parentRect;
    public RectTransform mapParentRect;
    public RectTransform playerAreaRect;

    private Image[,] mapImageArr;
    private Image playerAreaImage = null;

    private Color showColor = new Color(1, 1, 1, 1);
    private Color hideColor = new Color(0, 0, 0, 0);

    private void Awake()
    {
        Instance = this;

        mapImageArr = new Image[10, 10];

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject obj = Instantiate(dropTooltipObj, mapParentRect);

                Image image = obj.GetComponent<Image>();
                image.color = hideColor;

                mapImageArr[y, x] = image;
            }
        }
    }

    public void Show(bool b, CardType cardType)
    {
        if(b == false)
        {
            Hide();
            return;
        }


        //// 필드, 맵(건물), 특수카드(사용처에 따라 필드에 basicType에 따라 켜, 퀵슬롯, 플레이어
        //DropAreaType dropAreaType = DropAreaType.NULL;

        switch (cardType)
        {
            case CardType.Basic:
                List<Vector2> tempPointList = GameManager.Instance.fieldController.GetTempNextFieldPoint();
                foreach(Vector2 point in tempPointList)
                {
                    mapImageArr[(int)point.y, (int)point.x].color = showColor;
                }
                break;
            case CardType.Special:
                // 스페셜 카드의 타겟에 맞게 앞에 4개, 플레이어, 건물 등등
                break;
            case CardType.Build:
                foreach(DropArea dropArea in DropArea.dropAreas)
                {
                    if(dropArea.dropAreaType == DropAreaType.Build && dropArea.rectTrm.childCount == 0)
                    {
                        Vector2 point = dropArea.point;
                        mapImageArr[(int)point.y, (int)point.x].color = showColor;
                    }
                }
                break;
        }

        //foreach (var area in dropAreas)
        //{
        //    area.gameObject.SetActive(enable);
        //    if (area.dropAreaType == DropAreaType.NULL)
        //    {

        //    }
        //    // 같은 dropAreaType인지
        //    else if (area.dropAreaType == dropAreaType)
        //    {
        //        // feild라면 feildType이 able인지
        //        if (area.field != null)
        //        {
        //            if (area.field.fieldState == FieldState.able && area.field.transform.childCount == 0)
        //            {
        //                area.image.color = new Color(1, 1, 1, 1);
        //            }
        //            else
        //            {
        //                area.image.color = new Color(1, 1, 1, 0);
        //            }
        //        }
        //        // build라면 이미 설치된게있는지
        //        else
        //        {
        //            if (area.rectTrm.childCount == 0)
        //            {
        //                area.image.color = new Color(1, 1, 1, 1);
        //            }
        //            else
        //            {
        //                area.image.color = new Color(1, 1, 1, 0);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        area.image.color = new Color(1, 1, 1, 0);
        //    }
        //}
    }

    public void Hide()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                mapImageArr[y, x].color = hideColor;
            }
        }
        //playerAreaImage.color = hideColor;
    }
}
