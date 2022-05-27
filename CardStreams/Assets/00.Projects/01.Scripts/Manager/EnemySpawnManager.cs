using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject cardPrefab;

    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;

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
}
