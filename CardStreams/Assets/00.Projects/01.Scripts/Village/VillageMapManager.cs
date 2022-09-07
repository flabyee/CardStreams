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
    [HideInInspector] public RectTransform[,] mapRectArr;         // 전체 맵 배열
    public List<Vector2> nearRoadPointList = new List<Vector2>();
    [HideInInspector] public List<Field> fieldList = new List<Field>();   // 필드(플레이어가 가는 길)리스트
    [HideInInspector] public List<FieldData> sortFieldRectList = new List<FieldData>();   // 정렬할라고 임시로 값 저장하는 리스트, fieldList를 쓰면된다

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

    // 맵 생성 관련
    public void CreateMap(string mapStr)
    {
        mapRectArr = new RectTransform[10, 10];
        string[] mapStrArr = mapStr.Split(',');

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                RectTransform rectTrm = null;

                // 빈 곳(건물설치하는 곳)이라면
                if (mapStrArr[y * 10 + x] == "0")
                {
                    rectTrm = Instantiate(buildRectPrefab, mapRectParent).GetComponent<RectTransform>();
                }
                else
                {
                    // 필드(플레이어가 움직이는 곳)라면
                    rectTrm = Instantiate(fieldRectPrefab, mapRectParent).GetComponent<RectTransform>();

                    GameObject dropAreaObj = Instantiate(fieldDropPrefab, mapDropParent);

                    // 필드 정렬하기 위해서
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

        // 필드의 스프라이트는 어떻게 할것인가 : null로 하고 타일맵 찍어버리기~
        for (int i = 0; i < fieldList.Count; i++)
        {
            fieldList[i].background.sprite = null; // ConstManager.Instance.fieldSprites[0];
            //fieldList[i].background.color = new Color(1,1,1,0);
            fieldList[i].tileNum = 0;
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

        VillageGameManager.Instance.Init();
    }

    public RectTransform GetMapRectTrm(int y, int x)
    {
        return mapRectArr[y, x];
    }
}
