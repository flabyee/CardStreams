using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstallManager : MonoBehaviour
{
    private List<BuildSO> enemyBuildList;
    [SerializeField] GameObject buildPrefab;

    public void RandomEnemyBuild()
    {
        Vector2 point = MapManager.Instance.RandomMapIndex(); // 깔고자 하는 타일 위치

        int randomIndex = Random.Range(0, enemyBuildList.Count);
        EnemyBuildSO buildSO = enemyBuildList[randomIndex] as EnemyBuildSO;

        // Field가 생겼고 EnemyBuildSO(건물 정보)가 생겼다. 그럼 이제 뭐해야하나
        Build building = Instantiate(buildPrefab, MapManager.Instance.mapRectArr[(int)point.y, (int)point.x]).GetComponent<Build>();
        building.Init(buildSO);

        building.BuildDrop(point);
    }

    private void Awake()
    {
        BuildListSO buildListSO = Resources.Load<BuildListSO>("EnemyBuildListSO"); // 나중에 바꿔야댐

        enemyBuildList = buildListSO.buildList;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
