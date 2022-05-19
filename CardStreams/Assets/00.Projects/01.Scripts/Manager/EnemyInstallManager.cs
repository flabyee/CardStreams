using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstallManager : MonoBehaviour
{
    private List<BuildSO> enemyBuildList;
    [SerializeField] GameObject buildPrefab;

    public void RandomEnemyBuild()
    {
        Vector2 point = MapManager.Instance.RandomMapIndex(); // ����� �ϴ� Ÿ�� ��ġ

        int randomIndex = Random.Range(0, enemyBuildList.Count);
        EnemyBuildSO buildSO = enemyBuildList[randomIndex] as EnemyBuildSO;

        // Field�� ����� EnemyBuildSO(�ǹ� ����)�� �����. �׷� ���� ���ؾ��ϳ�
        Build building = Instantiate(buildPrefab, MapManager.Instance.mapRectArr[(int)point.y, (int)point.x]).GetComponent<Build>();
        building.Init(buildSO);

        building.BuildDrop(point);
    }

    private void Awake()
    {
        BuildListSO buildListSO = Resources.Load<BuildListSO>("EnemyBuildListSO"); // ���߿� �ٲ�ߴ�

        enemyBuildList = buildListSO.buildList;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
