using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("ParentRect")]
    public RectTransform mapRectParent;
    public RectTransform mapDropParent;

    [Header("Prefabs")]
    public GameObject buildRectPrefab;
    public GameObject buildDropPrefab;
    public GameObject fieldRectPrefab;
    public GameObject fieldDropPrefab;

    private System.Random random = new System.Random();

    public List<Vector2> fieldvectorList = new List<Vector2>();
    [HideInInspector] public RectTransform[,] mapRectArr;         // ��ü �� �迭
    public List<Vector2> nearRoadPointList = new List<Vector2>();
    [HideInInspector] public List<Field> fieldList = new List<Field>();   // �ʵ�(�÷��̾ ���� ��)����Ʈ
    [HideInInspector] public List<FieldData> sortFieldRectList = new List<FieldData>();   // �����Ҷ�� �ӽ÷� �� �����ϴ� ����Ʈ, fieldList�� ����ȴ�

    [Header("Event")]
    public EventSO afterMapCreateEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        CreateMap(DataManager.Instance.GetNowStageData().mapStr);
    }

    public void CreateMap(string mapStr)
    {
        mapRectArr = new RectTransform[10, 10];
        string[] mapStrArr = mapStr.Split(',');

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                //Debug.Log(y * 10 + x + " : " + mapStrArr[y * 10 + x]);
                RectTransform rectTrm = null;

                // �� ��(�ǹ���ġ�ϴ� ��)�̶��
                if (mapStrArr[y * 10 + x] == "0")
                {
                    rectTrm = Instantiate(buildRectPrefab, mapRectParent).GetComponent<RectTransform>();

                    GameObject dropAreaObj = Instantiate(buildDropPrefab, mapDropParent);

                    // dropArea - field ����
                    DropArea dropArea = dropAreaObj.GetComponent<DropArea>();
                    dropArea.rectTrm = rectTrm;


                    dropArea.point = new Vector2(x, y);

                }
                // �ʵ�(�÷��̾ �����̴� ��)���
                else
                {
                    rectTrm = Instantiate(fieldRectPrefab, mapRectParent).GetComponent<RectTransform>();

                    GameObject dropAreaObj = Instantiate(fieldDropPrefab, mapDropParent);


                    // dropArea - field ����
                    Field field = rectTrm.GetComponent<Field>();
                    DropArea dropArea = dropAreaObj.GetComponent<DropArea>();
                    field.dropArea = dropArea;
                    dropArea.field = field;
                    dropArea.rectTrm = rectTrm;
                    dropArea.point = new Vector2(x, y);
                    fieldvectorList.Add(field.dropArea.point); // ���� ���͸���Ʈ���߰�



                    // �ʵ� �����ϱ� ���ؼ�
                    int num = int.Parse(mapStrArr[y * 10 + x]);
                    //Debug.Log("int : " + num);
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

        afterMapCreateEvent.Occurred();
    }

    public RectTransform GetMapRectTrm(int y, int x)
    {
        return mapRectArr[y,x];
    }

    public Vector2 RandomMapIndex()
    {
        int rand = Random.Range(0, nearRoadPointList.Count);

        bool b = true;
        Vector2 point;
        while (b == true)
        {
            point = nearRoadPointList[0];
            if (mapRectArr[(int)point.y, (int)point.x].childCount == 0)
            {
                b = false;
                nearRoadPointList.RemoveAt(0);

                return point;
            }
            else
            {
                nearRoadPointList.RemoveAt(0);
            }
        }

        return Vector2.zero;
    }
}

// �� ������ �� �����ϱ� ���ؼ� �����ϴ� Ŭ����
public class FieldData
{
    public FieldData(int num, RectTransform rectTrm)
    {
        this.num = num;
        this.rectTrm = rectTrm;
    }

    public int num;
    public RectTransform rectTrm;
}