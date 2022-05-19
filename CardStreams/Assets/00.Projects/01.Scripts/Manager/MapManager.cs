using System.Collections;
using System.Collections.Generic;
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


    [HideInInspector] public RectTransform[,] mapRectArr;         // ��ü �� �迭
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

    void Update()
    {
        
    }

    public void CreateMap(string mapStr)
    {
        mapRectArr = new RectTransform[10, 10];
        string[] mapStrArr = mapStr.Split(',');

        for(int y = 0; y < 10; y++)
        {
            for(int x = 0; x < 10; x++)
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

                    // �ʵ� �����ϱ� ���ؼ�
                    int num = int.Parse(mapStrArr[y * 10 + x]);
                    //Debug.Log("int : " + num);
                    sortFieldRectList.Add(new FieldData(num, rectTrm));
                }

                mapRectArr[y, x] = rectTrm;
            }
        }

        sortFieldRectList.Sort((x, y) => x.num.CompareTo(y.num));

        foreach(FieldData fieldData in sortFieldRectList)
        {
            fieldList.Add(fieldData.rectTrm.GetComponent<Field>());
        }


        afterMapCreateEvent.Occurred();
    }

    public RectTransform RandomMapIndex()
    {
        int randX = Random.Range(0, mapRectArr.Length);
        int randY = Random.Range(0, mapRectArr.Length);

        return mapRectArr[randY, randX];
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
