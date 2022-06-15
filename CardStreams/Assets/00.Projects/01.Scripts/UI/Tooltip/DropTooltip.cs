using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTooltip : MonoBehaviour
{


    /// <summary>
    /// ������ ���� Show Hide active true false ���� color a�� ������
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

        // quikSlot�� playerArea�� width�� height�� ���ؼ� dropTooltipObj�� �����Ѵ�
    }

    public void Show(bool b, CardType cardType)
    {
        if(b == false)
        {
            Hide();
            return;
        }


        //// �ʵ�, ��(�ǹ�), Ư��ī��(���ó�� ���� �ʵ忡 basicType�� ���� ��, ������, �÷��̾�
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
                // ����� ī���� Ÿ�ٿ� �°� �տ� 4��, �÷��̾�, �ǹ� ���
                break;
            case CardType.Build:
                // ���� �ǹ��� ��ġ�������� �� ����
                break;
        }

        //foreach (var area in dropAreas)
        //{
        //    area.gameObject.SetActive(enable);
        //    if (area.dropAreaType == DropAreaType.NULL)
        //    {

        //    }
        //    // ���� dropAreaType����
        //    else if (area.dropAreaType == dropAreaType)
        //    {
        //        // feild��� feildType�� able����
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
        //        // build��� �̹� ��ġ�Ȱ��ִ���
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
