using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // �� ����
    public GameObject cardPrefab;

    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;

    // �� �ǹ� ����
    private List<BuildSO> enemyBuildList;
    [SerializeField] GameObject buildPrefab;
    [SerializeField] GameObject enemyBuildEffect;


    private void Start()
    {
        LoadStageData();
    }

    private void LoadStageData()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();
        mobSpawnAmount = stageData.firstMobSpawnAmount;
        mobSpawnIncreaseAmount = stageData.mobIncreaseAmount;
        mobAttackAmount = stageData.firstMobAttackAmount;
        mobAttackIncreaseAmount = stageData.mobAttackIncreaseAmount;

        enemyBuildList = DataManager.Instance.GetBuildSOList();
    }

    /// <summary>
    /// ���� �� ��ȯ
    /// </summary>
    public void CreateRandomMob()
    {
        Debug.Log("create random mob");

        List<int> canSpawnList = new List<int>();
        List<int> deleteFieldList = new List<int>();

        for (int i = 0; i < MapManager.Instance.GetFieldCount(); i++)
        {
            if (i == 0) // 0��°ĭ ��� ����
            {
                deleteFieldList.Add(0);
                continue;
            }

            canSpawnList.Add(i);
        }

        for (int i = 0; i < mobSpawnAmount; i++)
        {
            // ������ ������ �Ȼ��� �ֵ��� �־��ֱ�
            if (canSpawnList.Count <= 0)
            {
                // �����ϰ� ����
                for (int j = 0; j < deleteFieldList.Count; j++)
                {
                    int randomIndex = Random.Range(0, deleteFieldList.Count);
                    int temp = deleteFieldList[j];
                    deleteFieldList[j] = deleteFieldList[randomIndex];
                    deleteFieldList[randomIndex] = temp;
                }

                // ���� ����
                for (int j = i; j < mobSpawnAmount; j++)
                {
                    if (j >= MapManager.Instance.fieldList.Count)
                    {
                        return;
                    }
                    CreateEnemy(deleteFieldList[j - i]);
                }
                break;
            }

            int randIndex = canSpawnList[Random.Range(0, canSpawnList.Count)];
            CreateEnemy(randIndex);

            canSpawnList.Remove(randIndex);
            if (canSpawnList.Contains(randIndex + 1))
            {
                deleteFieldList.Add(randIndex + 1);
                canSpawnList.Remove(randIndex + 1);
            }
            if (canSpawnList.Contains(randIndex - 1))
            {
                deleteFieldList.Add(randIndex - 1);
                canSpawnList.Remove(randIndex - 1);
            }
        }

        mobSpawnAmount += mobSpawnIncreaseAmount;
        mobAttackAmount += mobAttackIncreaseAmount;
    }

    // <summary> ���� Ư�� ĭ�� ���͸� �����մϴ�. </summary>
    // <param name = "fieldIndex" > ������ ĭ</param>
    private void CreateEnemy(int fieldIndex)
    {
        int value = mobAttackAmount; // �����Ǵ� ������ ��

        // ���ο� ī�� ����
        GameObject cardObj = Instantiate(cardPrefab, MapManager.Instance.fieldList[fieldIndex].transform);
        DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
        CardPower cardPower = cardObj.GetComponent<CardPower>();

        // cardPower�� ���� �ֱ�
        dragbleCard.SetData_Feild(CardType.Monster, value);

        // �� �����̰�
        dragbleCard.canDragAndDrop = false;

        // �ʵ忡 ���� + not����
        MapManager.Instance.fieldList[fieldIndex].Init(cardPower, dragbleCard, FieldState.not);

        // ���� ����
        cardPower.backImage.color = Color.magenta;

        // craete effect
        EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
    }



    /// <summary>
    /// �ǹ�
    /// </summary>
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
