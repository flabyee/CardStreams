using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstallManager : MonoBehaviour
{
    private List<BuildSO> enemyBuildList;

    public void RandomEnemyBuild()
    {
        // RectTransform tile = MapManager.Instance.RandomMapIndex();

        // Debug.Log();

        int randomIndex = Random.Range(0, enemyBuildList.Count);
        EnemyBuildSO buildSO = enemyBuildList[randomIndex] as EnemyBuildSO;
    }

    private void Awake()
    {
        BuildListSO buildListSO = Resources.Load<BuildListSO>("EnemyBuildListSO"); // ³ªÁß¿¡ ¹Ù²ã¾ß´ï

        enemyBuildList = buildListSO.buildList;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
