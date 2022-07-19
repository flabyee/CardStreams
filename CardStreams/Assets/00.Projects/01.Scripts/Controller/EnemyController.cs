using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class EnemyController : MonoBehaviour
{
    // �� ����
    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;
    private int bossValue;

    // �� �ǹ� ����
    private List<BuildSO> enemyBuildList;

    public IntValue loopCountValue;

    public GameObject bossSpawnImage;

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
        bossValue = stageData.bossValue;
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
        dragbleCard.InitData_Feild(BasicType.Monster, value);

        // �� �����̰�
        dragbleCard.canDragAndDrop = false;
        cardPower.SetField();

        Field field = MapManager.Instance.fieldList[fieldIndex];

        // ��ġ&�θ� ����
        dragbleCard.transform.position = field.transform.position;
        dragbleCard.transform.SetParent(field.transform);

        // �ʵ忡 ���� + not����
        field.Init(cardPower, dragbleCard, FieldState.randomMob);

        dragbleCard.SetDroppedArea(field.dropArea);

        // craete effect
        //EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
        Effects.Instance.TriggerTeleport(MapManager.Instance.fieldList[fieldIndex].transform.position);

        (cardPower as BasicCard).OnField();

    }



    /// <summary>
    /// �ǹ�
    /// </summary>
    public void RandomEnemyBuild()
    {
        if (loopCountValue.RuntimeValue == 0)
            return;

        for(int i = 0; i < loopCountValue.RuntimeValue / 2 + 2; i++)
        {
            // ��ġ�� ��ġ
            Vector2 randomPoint = MapManager.Instance.RandomMapIndex(); // ���� ��ġ ȹ��
            RectTransform rectTrm = MapManager.Instance.GetMapRectTrm((int)randomPoint.y, (int)randomPoint.x); // ���������� ��ġ�� ��ġ �ۿ���

            // ��ġ�� �ǹ�
            int randomIndex = Random.Range(0, enemyBuildList.Count); // ���� �ǹ� ȹ��
            EnemyBuildSO buildSO = enemyBuildList[randomIndex] as EnemyBuildSO; // ���������� ��ġ�Ұǹ� �ۿ���

            // �ǹ���ġ
            BuildCard building = CardPoolManager.Instance.GetBuildCard(rectTrm).GetComponent<BuildCard>();
            building.transform.position = rectTrm.position;

            Effects.Instance.TriggerTeleport(rectTrm.transform.position);

            building.Init(buildSO);

            building.BuildDrop(randomPoint);


            // �� ����
            CardPower cardPower = building.GetComponent<CardPower>();
            cardPower.backImage.color = Color.magenta;
            cardPower.OnField();
        }
    }

    public void BossRound()
    {
        mobSpawnAmount /= 2;
        mobAttackAmount /= 2;

        CreateRandomMob();
        RandomEnemyBuild();
        // RandomEnemyBuild();
        // RandomEnemyBuild();

        CreateBoss(MapManager.Instance.fieldCount - 1);
    }

    public void CreateBoss(int fieldIndex)
    {
        int value = bossValue;

        // ���ο� ī�� ���� + �θ� ����
        GameObject cardObj = CardPoolManager.Instance.GetBasicCard(MapManager.Instance.fieldList[fieldIndex].transform);
        DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
        CardPower cardPower = cardObj.GetComponent<CardPower>();

        (cardPower as BasicCard).isBoss = true;

        // cardPower�� ���� �ֱ�
        dragbleCard.InitData_Feild(BasicType.Monster, value);

        // �� �����̰�
        dragbleCard.canDragAndDrop = false;
        cardPower.SetField();

        Field field = MapManager.Instance.fieldList[fieldIndex];

        // ��ġ&�θ� ����
        dragbleCard.transform.position = field.transform.position;
        dragbleCard.transform.SetParent(field.transform);

        // �ʵ忡 ���� + not����
        field.Init(cardPower, dragbleCard, FieldState.randomMob);

        dragbleCard.SetDroppedArea(field.dropArea);

        // craete effect
        //EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
        Effects.Instance.TriggerTeleport(MapManager.Instance.fieldList[fieldIndex].transform.position);

        (cardPower as BasicCard).OnField();

        bossSpawnImage.SetActive(true);
        bossSpawnImage.GetComponent<Image>().DOFade(1f, 0f);
        bossSpawnImage.GetComponent<Image>().DOFade(0f, 2f);
        StartCoroutine(Temp(2f));
       
    }

    IEnumerator Temp(float delay)
    {
        yield return new WaitForSeconds(delay);
        bossSpawnImage.SetActive(false);
    }
}