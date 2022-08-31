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

    [SerializeField] GameObject square;
    private Vector3 firstSquarePoint;
    private Vector3 lastSquarePoint;

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
        CreateMap(DataManager.Instance.GetNowStageData().mapStr);
    }

    // �� ���� ����
    public void CreateMap(string mapStr)
    {
        mapRectArr = new RectTransform[10, 10];
        string[] mapStrArr = mapStr.Split(',');

        int squareCount = 0;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
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
                else if(mapStrArr[y * 10 + x] == "s")
                {
                    squareCount++;

                    rectTrm = Instantiate(fieldRectPrefab, mapRectParent).GetComponent<RectTransform>();

                    GameObject dropAreaObj = Instantiate(fieldDropPrefab, mapDropParent);
                    Debug.Log(dropAreaObj.transform.position);

                    DropArea dropArea = dropAreaObj.GetComponent<DropArea>();
                    dropArea.rectTrm = rectTrm;
                    dropArea.point = new Vector2(x, y);

                    if (squareCount == 1)
                    {
                        Debug.Log("first");
                        firstSquarePoint = rectTrm.position;
                    }
                    lastSquarePoint = rectTrm.position;
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

        // �ʵ� ��׶��� ������ ���� ��ȣ�ű��
        int tileNum = 0;
        for (int i = 0; i < fieldList.Count / 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                fieldList[i * 4 + j].background.sprite = ConstManager.Instance.fieldSprites[tileNum];
                fieldList[i * 4 + j].tileNum = tileNum;
            }

            tileNum = tileNum == 0 ? 1 : 0;
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
    }

    public RectTransform GetMapRectTrm(int y, int x)
    {
        return mapRectArr[y, x];
    }
}
