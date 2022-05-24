using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstallManager : MonoBehaviour
{
    private List<BuildSO> enemyBuildList;
    [SerializeField] GameObject buildPrefab;
    [SerializeField] GameObject enemyBuildEffect;

    private void Awake()
    {
        BuildListSO buildListSO = Resources.Load<BuildListSO>("EnemyBuildListSO"); // 나중에 바꿔야댐

        enemyBuildList = buildListSO.buildList;
    }

    public void RandomEnemyBuild()
    {
        // 설치할 위치
        Vector2 randomPoint = MapManager.Instance.RandomMapIndex(); // 랜덤 위치 획득
        RectTransform buildPoint = MapManager.Instance.GetMapRectTrm((int)randomPoint.y, (int)randomPoint.x); // 랜덤값으로 설치할 위치 퍼오기

        // 설치할 건물
        int randomIndex = Random.Range(0, enemyBuildList.Count); // 랜덤 건물 획득
        EnemyBuildSO buildSO = enemyBuildList[randomIndex] as EnemyBuildSO; // 랜덤값으로 설치할건물 퍼오기

        // 건물설치
        Build building = Instantiate(buildPrefab, buildPoint).GetComponent<Build>();
        GameObject clone = Instantiate(enemyBuildEffect, buildPoint.transform.position, Quaternion.identity); // rectTransform ???
        Destroy(clone, 1f);
        
        building.Init(buildSO);

        building.BuildDrop(randomPoint);

        // 색 변경
        CardPower cardPower = building.GetComponent<CardPower>();
        cardPower.backImage.color = Color.magenta;
    }

}
