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
    [HideInInspector] public RectTransform[,] mapRectArr;         // 전체 맵 배열
    public List<Vector2> nearRoadPointList = new List<Vector2>();
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
                    fieldvectorList.Add(field.dropArea.point); // 도로 벡터리스트에추가



                    // 필드 정렬하기 위해서
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

        // 다시 쫙 훑으면서 도로와 인접한 타일을 특별 List에 추가
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (fieldvectorList.Contains(new Vector2(x, y))) continue;

                for (int count = 0; count < 8; count++)
                {
                    // 검사하던중 도로가 있다? count for문 break
                    if (fieldvectorList.Contains(new Vector2(x + nearPoints[count].x, y + nearPoints[count].y))) // 도로 리스트에 좌표가 들어가있으면
                    {
                        // 현재 x y 좌표 근처 8칸에 도로가 있음!
                        nearRoadPointList.Add(new Vector2(x, y));
                        break;
                    }
                }
            }
        }

        int randomIndex = 0;

        for (int i = 0; i < nearRoadPointList.Count; i++)
        {
            randomIndex = Random.Range(0, nearRoadPointList.Count); // 랜덤인덱스 뽑아서

            Vector2 temp = nearRoadPointList[i]; // i랑 랜덤인덱스랑 스왑
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