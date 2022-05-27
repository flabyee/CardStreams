using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 적 생성
    public GameObject cardPrefab;

    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;

    // 적 건물 생성
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
    /// 랜덤 적 소환
    /// </summary>
    public void CreateRandomMob()
    {
        Debug.Log("create random mob");

        List<int> canSpawnList = new List<int>();
        List<int> deleteFieldList = new List<int>();

        for (int i = 0; i < MapManager.Instance.GetFieldCount(); i++)
        {
            if (i == 0) // 0번째칸 억까 방지
            {
                deleteFieldList.Add(0);
                continue;
            }

            canSpawnList.Add(i);
        }

        for (int i = 0; i < mobSpawnAmount; i++)
        {
            // 범위에 못들어가서 안뽑힌 애들을 넣어주기
            if (canSpawnList.Count <= 0)
            {
                // 랜덤하게 셔플
                for (int j = 0; j < deleteFieldList.Count; j++)
                {
                    int randomIndex = Random.Range(0, deleteFieldList.Count);
                    int temp = deleteFieldList[j];
                    deleteFieldList[j] = deleteFieldList[randomIndex];
                    deleteFieldList[randomIndex] = temp;
                }

                // 몬스터 생성
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

    // <summary> 맵의 특정 칸에 몬스터를 생성합니다. </summary>
    // <param name = "fieldIndex" > 생성할 칸</param>
    private void CreateEnemy(int fieldIndex)
    {
        int value = mobAttackAmount; // 생성되는 몬스터의 값

        // 새로운 카드 생성
        GameObject cardObj = Instantiate(cardPrefab, MapManager.Instance.fieldList[fieldIndex].transform);
        DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
        CardPower cardPower = cardObj.GetComponent<CardPower>();

        // cardPower에 정보 넣기
        dragbleCard.SetData_Feild(CardType.Monster, value);

        // 못 움직이게
        dragbleCard.canDragAndDrop = false;

        // 필드에 적용 + not으로
        MapManager.Instance.fieldList[fieldIndex].Init(cardPower, dragbleCard, FieldState.not);

        // 배경색 변경
        cardPower.backImage.color = Color.magenta;

        // craete effect
        EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
    }



    /// <summary>
    /// 건물
    /// </summary>
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
