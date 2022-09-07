using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageMapManager : MonoBehaviour
{
    public static VillageMapManager Instance;

    [Header("ParentRect")]
    public RectTransform mapRectParent;
    public RectTransform mapDropParent;

    [Header("Prefabs")]
    public GameObject buildRectPrefab;
    public GameObject buildDropPrefab;
    public GameObject fieldRectPrefab;
    public GameObject fieldDropPrefab;

    public List<Vector2> fieldvectorList = new List<Vector2>();
    [HideInInspector] public RectTransform[,] mapRectArr;         // ��ü �� �迭
    public List<Vector2> nearRoadPointList = new List<Vector2>();
    [HideInInspector] public List<Field> fieldList = new List<Field>();   // �ʵ�(�÷��̾ ���� ��)����Ʈ
    [HideInInspector] public List<FieldData> sortFieldRectList = new List<FieldData>();   // �����Ҷ�� �ӽ÷� �� �����ϴ� ����Ʈ, fieldList�� ����ȴ�

    public int fieldCount => fieldList.Count;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        CreateMap(DataManager.Instance.GetStageData(5).mapStr);
    }

    // �� ���� ����
    public void CreateMap(string mapStr)
    {
        mapRectArr = new RectTransform[10, 10];
        string[] mapStrArr = mapStr.Split(',');

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                RectTransform rectTrm = null;

                // �� ��(�ǹ���ġ�ϴ� ��)�̶��
                if (mapStrArr[y * 10 + x] == "0")
                {
                    rectTrm = Instantiate(buildRectPrefab, mapRectParent).GetComponent<RectTransform>();
                }
                else
                {
                    // �ʵ�(�÷��̾ �����̴� ��)���
                    rectTrm = Instantiate(fieldRectPrefab, mapRectParent).GetComponent<RectTransform>();

                    GameObject dropAreaObj = Instantiate(fieldDropPrefab, mapDropParent);

                    // �ʵ� �����ϱ� ���ؼ�
                    int num = int.Parse(mapStrArr[y * 10 + x]);
                    sortFieldRectList.Add(new FieldData(num, rectTrm));
                }

                mapRectArr[y, x] = rectTrm;
            }
        }

        sortFieldRectList.Sort((x, y) => x.num.CompareTo(y.num));

        foreach (FieldData fieldData in sortFieldRectList)
        {
            fieldList.Add(fieldData.rectTrm.GetComponent<Field>());
        }

        // �ʵ��� ��������Ʈ�� ��� �Ұ��ΰ� : null�� �ϰ� Ÿ�ϸ� ��������~
        for (int i = 0; i < fieldList.Count; i++)
        {
            fieldList[i].background.sprite = null; // ConstManager.Instance.fieldSprites[0];
            //fieldList[i].background.color = new Color(1,1,1,0);
            fieldList[i].tileNum = 0;
        }

        Vector2Int[] nearPoints = new Vector2Int[] {
        new Vector2Int(-1,1), new Vector2Int(0,  1), new Vector2Int(1, 1), new Vector2Int(-1, 0),
        new Vector2Int(1, 0), new Vector2Int(-1,-1), new Vector2Int(0,-1), new Vector2Int(1, -1)};

        // �ٽ� �� �����鼭 ���ο� ������ Ÿ���� Ư�� List�� �߰�
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (fieldvectorList.Contains(new Vector2(x, y))) continue;

                for (int count = 0; count < 8; count++)
                {
                    // �˻��ϴ��� ���ΰ� �ִ�? count for�� break
                    if (fieldvectorList.Contains(new Vector2(x + nearPoints[count].x, y + nearPoints[count].y))) // ���� ����Ʈ�� ��ǥ�� ��������
                    {
                        // ���� x y ��ǥ ��ó 8ĭ�� ���ΰ� ����!
                        nearRoadPointList.Add(new Vector2(x, y));
                        break;
                    }
                }
            }
        }

        int randomIndex = 0;

        for (int i = 0; i < nearRoadPointList.Count; i++)
        {
            randomIndex = Random.Range(0, nearRoadPointList.Count); // �����ε��� �̾Ƽ�

            Vector2 temp = nearRoadPointList[i]; // i�� �����ε����� ����
            nearRoadPointList[i] = nearRoadPointList[randomIndex];
            nearRoadPointList[randomIndex] = temp;
        }

        VillageGameManager.Instance.Init();
    }

    public RectTransform GetMapRectTrm(int y, int x)
    {
        return mapRectArr[y, x];
    }
}
