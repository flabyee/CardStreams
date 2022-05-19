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


    [HideInInspector] public RectTransform[,] mapRectArr;         // 전체 맵 배열
    [HideInInspector] public List<Field> fieldList = new List<Field>();   // 필드(플레이어가 가는 길)리스트
    [HideInInspector] public List<FieldData> sortFieldRectList = new List<FieldData>();   // 정렬할라고 임시로 값 저장하는 리스트, fieldList를 쓰면된다

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

                // 빈 곳(건물설치하는 곳)이라면
                if (mapStrArr[y * 10 + x] == "0")
                {
                    rectTrm = Instantiate(buildRectPrefab, mapRectParent).GetComponent<RectTransform>();

                    GameObject dropAreaObj = Instantiate(buildDropPrefab, mapDropParent);

                    // dropArea - field 연결
                    DropArea dropArea = dropAreaObj.GetComponent<DropArea>();
                    dropArea.rectTrm = rectTrm;


                    dropArea.point = new Vector2(x, y);
                }
                // 필드(플레이어가 움직이는 곳)라면
                else
                {
                    rectTrm = Instantiate(fieldRectPrefab, mapRectParent).GetComponent<RectTransform>();

                    GameObject dropAreaObj = Instantiate(fieldDropPrefab, mapDropParent);


                    // dropArea - field 연결
                    Field field = rectTrm.GetComponent<Field>();
                    DropArea dropArea = dropAreaObj.GetComponent<DropArea>();
                    field.dropArea = dropArea;
                    dropArea.field = field;
                    dropArea.rectTrm = rectTrm;


                    dropArea.point = new Vector2(x, y);

                    // 필드 정렬하기 위해서
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

// 맵 생성할 때 정렬하기 위해서 존재하는 클래스
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
