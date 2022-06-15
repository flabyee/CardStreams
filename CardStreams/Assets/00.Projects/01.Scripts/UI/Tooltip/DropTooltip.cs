using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTooltip : MonoBehaviour
{


    /// <summary>
    /// 다음에 오면 Show Hide active true false 말고 color a로 ㄱㄱㄱ
    /// </summary>
    public static DropTooltip Instance;

    public GameObject dropTooltipObj;
    public RectTransform parentRect;
    public RectTransform mapParentRect;
    public RectTransform quikSlotParentRect;
    public List<RectTransform> quikSlotRectList;
    public RectTransform playerAreaRect;

    private RectTransform[,] mapRectArr;
    private RectTransform[] quikSlotArr;

    private void Awake()
    {
        Instance = this;

        mapRectArr = new RectTransform[10, 10];
        quikSlotArr = new RectTransform[10];

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject obj = Instantiate(dropTooltipObj, mapParentRect);
                obj.SetActive(false);

                RectTransform rectTrm = obj.GetComponent<RectTransform>();

                mapRectArr[y, x] = rectTrm;
            }
        }

        for(int i = 0; i < quikSlotRectList.Count; i++)
        {
            GameObject obj = Instantiate(dropTooltipObj, quikSlotParentRect);
            obj.SetActive(false);

            RectTransform rectTrm = obj.GetComponent<RectTransform>();

            quikSlotArr[i] = rectTrm;
        }

        // quikSlot과 playerArea는 width와 height를 구해서 dropTooltipObj를 생성한다
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
                    mapRectArr[(int)point.x, (int)point.y].gameObject.SetActive(true);
                }
                break;
            case CardType.Special:
                // 스페셜 카드의 타겟에 맞게 앞에 4개, 플레이어, 건물 등등
                break;
            case CardType.Build:
                // 아직 건물이 배치되지않은 맵 전부
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

    }
}
