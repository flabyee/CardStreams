using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // �� ����
    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;

    // �� �ǹ� ����
    private List<BuildSO> enemyBuildList;

    private void Awake()
    {
        enemyBuildList = Resources.Load<BuildListSO>("EnemyBuildListSO").buildList;

    }


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

    }

    /// <summary>
    /// ���� �� ��ȯ
    /// </summary>
    public void CreateRandomMob()
    {
        List<int> canSpawnList = new List<int>();
        List<int> deleteFieldList = new List<int>();

        for (int i = 0; i < MapManager.Instance.fieldCount; i++)
        {
            if (i == 0 || i == MapManager.Instance.fieldCount - 1) // 0��°ĭ ��� ����
            {
                deleteFieldList.Add(i);
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
                    if (j >= MapManager.Instance.fieldCount)
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
            if (canSpawnList.Contains(randIndex + 2))
            {
                deleteFieldList.Add(randIndex + 2);
                canSpawnList.Remove(randIndex + 2);
            }
            if (canSpawnList.Contains(randIndex - 2))
            {
                deleteFieldList.Add(randIndex - 2);
                canSpawnList.Remove(randIndex - 2);
            }
            if (canSpawnList.Contains(randIndex + 3))
            {
                deleteFieldList.Add(randIndex + 3);
                canSpawnList.Remove(randIndex + 3);
            }
            if (canSpawnList.Contains(randIndex - 3))
            {
                deleteFieldList.Add(randIndex - 3);
                canSpawnList.Remove(randIndex - 3);
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

        // ���ο� ī�� ���� + �θ� ����
        GameObject cardObj = CardPoolManager.Instance.GetBasicCard(MapManager.Instance.fieldList[fieldIndex].transform);
        DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
        CardPower cardPower = cardObj.GetComponent<CardPower>();

        // cardPower�� ���� �ֱ�
        dragbleCard.SetData_Feild(BasicType.Monster, value);

        // �� �����̰�
        dragbleCard.canDragAndDrop = false;
        cardPower.SetField();

        // �ʵ忡 ���� + not����
        MapManager.Instance.fieldList[fieldIndex].Init(cardPower, dragbleCard, FieldState.not);

        // ���� ����
        cardPower.backImage.color = Color.magenta;

        // craete effect
        //EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
        Effects.Instance.TriggerTeleport(MapManager.Instance.fieldList[fieldIndex].transform.position);
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
        BuildCard building = CardPoolManager.Instance.GetBuildCard(buildPoint).GetComponent<BuildCard>();

        Effects.Instance.TriggerTeleport(buildPoint.transform.position);

        building.Init(buildSO);

        building.BuildDrop(randomPoint);

        // �� ����
        CardPower cardPower = building.GetComponent<CardPower>();
        cardPower.backImage.color = Color.magenta;
    }

}
