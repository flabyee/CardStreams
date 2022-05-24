using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ġ�Ǿ��ְų� �巡�� ����ϴ� �ǹ��� ������ ǥ���� �� ���
public class BuildAreaTooltip : MonoBehaviour
{
    public static BuildAreaTooltip Instance;

    public GameObject areaTooltipPrefab;

    private Image[,] areaImageArr = new Image[5, 5];
    private Transform followTrm;
    private bool isFollow;

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

    private void Update()
    {
        if(isFollow && followTrm != null)
        {
            transform.position = followTrm.position;
        }
    }

    public void Show(Vector3 pos, List<Vector2> accessPointList)
    {
        transform.position = pos;

        for (int y = 2; y >= -2; y--)
        {
            for (int x = -2; x <= 2; x++)
            {
                // point�� �ش�Ǵ� �����̾ƴ϶�� Alpha = 0
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

    public void ShowFollow(Transform followTrm, List<Vector2> accessPointList)
    {
        Show(transform.position, accessPointList);

        this.isFollow = true;
        this.followTrm = followTrm;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void HideFollow()
    {
        Hide();

        this.isFollow = false;
        this.followTrm = null;
    }
}
