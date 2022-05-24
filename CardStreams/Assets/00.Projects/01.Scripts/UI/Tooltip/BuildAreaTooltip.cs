using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildAreaTooltip : MonoBehaviour
{
    public static BuildAreaTooltip Instance;

    public GameObject areaTooltipPrefab;
    private Image[,] areaImageArr = new Image[5, 5];
    private void Awake()
    {
        Hide();
        Instance = this;

        for (int y = 2; y >= -2; y--)
        {
            for (int x = -2; x <= 2; x++)
            {
                GameObject obj = Instantiate(areaTooltipPrefab, transform);

                Image image = obj.GetComponent<Image>();

                areaImageArr[y + 2, x + 2] = image;
            }
        }
    }

    public void Show(Vector3 pos, List<Vector2> accessPointList)
    {
        transform.position = pos;

        for (int y = 2; y >= -2; y--)
        {
            for (int x = -2; x <= 2; x++)
            {
                // point에 해당되는 구역이아니라면 Alpha = 0
                if (!accessPointList.Contains(new Vector2(x, y)))
                {
                    areaImageArr[y + 2, x + 2].color = new Color(0, 0, 0, 0);
                }
                else
                {
                    areaImageArr[y + 2, x + 2].color = Color.green;
                }

                if (y == 0 && x == 0)
                {
                    
                }
            }
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
