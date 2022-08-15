using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class EnemyController : MonoBehaviour
{

    // 적 생성
    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;

    // 적 건물 생성
    private List<BuildSO> enemyBuildList;

    public IntValue loopCountValue;

    public GameObject bossSpawnImage;

    // 보스 관련
    public Transform bossParentTrm;
    public GameObject bossPrefab;
    private GameObject bossObj;
    private TextMeshProUGUI bossValueText;
    private int bossHp;
    private int bossAtk;

    public Action<int> bossMoveEvnet;


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

        bossHp = stageData.bossHP;
        bossAtk = stageData.bossAtk;
    }

    /// <summary>
    /// 랜덤 적 소환
    /// </summary>
    public void CreateRandomMob()
    {
        List<int> canSpawnList = new List<int>();
        List<int> deleteFieldList = new List<int>();

        for (int i = 0; i < MapManager.Instance.fieldCount; i++)
        {
            if (i == 0 || i == MapManager.Instance.fieldCount - 1) // 0번째칸 억까 방지
            {
                deleteFieldList.Add(i);
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
                    int randomIndex = UnityEngine.Random.Range(0, deleteFieldList.Count);
                    int temp = deleteFieldList[j];
                    deleteFieldList[j] = deleteFieldList[randomIndex];
                    deleteFieldList[randomIndex] = temp;
                }

                // 몬스터 생성
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

            int randIndex = canSpawnList[UnityEngine.Random.Range(0, canSpawnList.Count)];
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

    // <summary> 맵의 특정 칸에 몬스터를 생성합니다. </summary>
    // <param name = "fieldIndex" > 생성할 칸</param>
    private void CreateEnemy(int fieldIndex)
    {
        int value = mobAttackAmount; // 생성되는 몬스터의 값

        // 새로운 카드 생성 + 부모 설정
        GameObject cardObj = CardPoolManager.Instance.GetBasicCard(MapManager.Instance.fieldList[fieldIndex].transform);
        DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
        CardPower cardPower = cardObj.GetComponent<CardPower>();

        // cardPower에 정보 넣기
        dragbleCard.InitData_Feild(BasicType.Monster, value);

        // 못 움직이게
        dragbleCard.canDragAndDrop = false;
        cardPower.SetField();

        Field field = MapManager.Instance.fieldList[fieldIndex];

        // 위치&부모 설정
        dragbleCard.transform.position = field.transform.position;
        dragbleCard.transform.SetParent(field.transform);

        // 필드에 적용 + not으로
        field.Init(cardPower, dragbleCard, FieldState.randomMob);

        dragbleCard.SetDroppedArea(field.dropArea);

        // craete effect
        //EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
        Effects.Instance.TriggerTeleport(MapManager.Instance.fieldList[fieldIndex].transform.position);

        (cardPower as BasicCard).OnField();

    }



    /// <summary>
    /// 건물
    /// </summary>
    public void CreateEnemyBuild()
    {
        if (loopCountValue.RuntimeValue == 0)
            return;

        for(int i = 0; i < loopCountValue.RuntimeValue / 2 + 2; i++)
        {
            // 설치할 위치
            Vector2 randomPoint = MapManager.Instance.RandomMapIndex(); // 랜덤 위치 획득
            RectTransform rectTrm = MapManager.Instance.GetMapRectTrm((int)randomPoint.y, (int)randomPoint.x); // 랜덤값으로 설치할 위치 퍼오기

            // 설치할 건물
            int randomIndex = UnityEngine.Random.Range(0, enemyBuildList.Count); // 랜덤 건물 획득
            EnemyBuildSO buildSO = enemyBuildList[randomIndex] as EnemyBuildSO; // 랜덤값으로 설치할건물 퍼오기

            // 건물설치
            BuildCard building = CardPoolManager.Instance.GetBuildCard(rectTrm).GetComponent<BuildCard>();
            building.transform.position = rectTrm.position;

            Effects.Instance.TriggerTeleport(rectTrm.transform.position);

            building.Init(buildSO);

            building.BuildDrop(randomPoint);


            // 색 변경
            CardPower cardPower = building.GetComponent<CardPower>();
            cardPower.backImage.color = Color.magenta;
            cardPower.OnField();
        }
    }

    public void BossRound()
    {
        CreateRandomMob();
        CreateEnemyBuild();

        CreateBoss();
    }

    public void CreateBoss()
    {
        // 보스 생성
        bossObj = Instantiate(bossPrefab, bossParentTrm);
        bossObj.transform.position = MapManager.Instance.fieldList[4].transform.position;

        bossValueText = bossObj.GetComponentInChildren<TextMeshProUGUI>();
        bossValueText.text = bossHp.ToString();

        Effects.Instance.TriggerTeleport(MapManager.Instance.fieldList[4].transform.position);

        // 연출
        bossSpawnImage.SetActive(true);
        bossSpawnImage.GetComponent<Image>().DOFade(1f, 0f);
        bossSpawnImage.GetComponent<Image>().DOFade(0f, 2f);
        StartCoroutine(Temp(2f));
    }

    public void MoveBoss(int moveIndex)
    {
        if(bossObj == null)
        {
            Debug.LogError("보스가 없는데 보스를 움직이려 했습니다");
            return;
        }

        Sequence sequence = DOTween.Sequence();

        for(int i = 1; i <= 4; i++)
        {
            int closerIndex = i;
            sequence.AppendCallback(() =>
            {
                bossObj.transform.DOMove(MapManager.Instance.fieldList[(moveIndex + closerIndex) % MapManager.Instance.fieldCount].transform.position, 1f);
                bossMoveEvnet?.Invoke(moveIndex + closerIndex);
            });
            sequence.AppendInterval(1f);
        }

        
    }

    public int GetBossAtk()
    {
        return bossAtk;
    }
    public GameObject GetBossObj()
    {
        return bossObj;
    }

    public void AttackBoss(int sword)
    {
        bossHp -= bossAtk + sword;

        // UI 갱신
        bossValueText.text = bossHp.ToString();

        if(bossHp <= 0)
        {

        }
    }

    IEnumerator Temp(float delay)
    {
        yield return new WaitForSeconds(delay);
        bossSpawnImage.SetActive(false);
    }
}