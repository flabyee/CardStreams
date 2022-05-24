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
        BuildListSO buildListSO = Resources.Load<BuildListSO>("EnemyBuildListSO"); // ���߿� �ٲ�ߴ�

        enemyBuildList = buildListSO.buildList;
    }

    public void RandomEnemyBuild()
    {
        // ��ġ�� ��ġ
        Vector2 randomPoint = MapManager.Instance.RandomMapIndex(); // ���� ��ġ ȹ��
        RectTransform buildPoint = MapManager.Instance.GetMapRectTrm((int)randomPoint.y, (int)randomPoint.x); // ���������� ��ġ�� ��ġ �ۿ���

        // ��ġ�� �ǹ�
        int randomIndex = Random.Range(0, enemyBuildList.Count); // ���� �ǹ� ȹ��
        EnemyBuildSO buildSO = enemyBuildList[randomIndex] as EnemyBuildSO; // ���������� ��ġ�Ұǹ� �ۿ���

        // �ǹ���ġ
        Build building = Instantiate(buildPrefab, buildPoint).GetComponent<Build>();
        GameObject clone = Instantiate(enemyBuildEffect, buildPoint.transform.position, Quaternion.identity); // rectTransform ???
        Destroy(clone, 1f);
        
        building.Init(buildSO);

        building.BuildDrop(randomPoint);

        // �� ����
        CardPower cardPower = building.GetComponent<CardPower>();
        cardPower.backImage.color = Color.magenta;
    }

}
