using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    /*0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,
0,0,0,1,2,3,0,0,0,0,
0,0,0,8,0,4,0,0,0,0,
0,0,0,7,6,5,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0*/


    public RectTransform mapRectParent;
    public RectTransform mapDropParent;

    public GameObject buildRectPrefab;
    public GameObject buildDropPrefab;
    public GameObject fieldRectPrefab;
    public GameObject fieldDropPrefab;


    public RectTransform[,] mapRectArr;         // ��ü �� �迭
    public List<Field> fieldList = new List<Field>();   // �ʵ�(�÷��̾ ���� ��)����Ʈ
    public List<FieldData> sortFieldRectList = new List<FieldData>();   // �ʵ�(�÷��̾ ���� ��)����Ʈ


    private List<DropArea> mapDropAreaList = new List<DropArea>();
    private List<RectTransform> mapRectList = new List<RectTransform>();

    private void Awake()
    {
        Instance = this;

        CreateMap("0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,0,0,8,0,4,0,0,0,0,0,0,0,7,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
    }

    void Start()
    {
        
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

                    Instantiate(buildDropPrefab, mapDropParent);
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


                    // �ʵ� �����ϱ� ���ؼ�
                    int num = int.Parse(mapStrArr[y * 10 + x]);
                    Debug.Log("int : " + num);
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
