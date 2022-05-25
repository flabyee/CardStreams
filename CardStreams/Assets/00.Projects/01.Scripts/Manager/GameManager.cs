using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public enum CardType
{
    NULL,
    Sword,
    Sheild,
    Potion,
    Monster,
    Coin,
    Special,
    Build,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isDebuging;

    [Header("System")]
    [SerializeField] float moveDuration;
    [SerializeField] float fieldResetDelay;
    private bool isMoving;  // move중일때 또 next를 누르지 못하게

    [HideInInspector]public int rerollCount;

    public bool canStartTurn;

    [Header("UI")]
    public Player player;
    public GameObject cardPrefab;

    // stageData
    private int maxMoveCount = 3;  // n
    private int moveIndex = 0;
    private int moveCount = 0;  // n번씩 움직일거다
    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;  // 몹 생성수 증가량
    private int mobAttackAmount;          // 몹 공격력
    private int mobAttackIncreaseAmount;    // 몹 공격력 증가량



    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue turnCountValue;
    

    [Header("Event")]
    public EventSO GameStartEvent;
    public EventSO TurnStartEvent;
    public EventSO TurnEndEvent;
    public EventSO MoveStartEvent;
    public EventSO MoveEndEvent;

    public EventSO goldChangeEvent;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if(isDebuging == true)
        {
            moveDuration = 0.05f;
        }
    }

    private void Start()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();
        maxMoveCount = stageData.moveCount;
        mobSpawnAmount = stageData.firstMobSpawnAmount;
        mobSpawnIncreaseAmount = stageData.mobIncreaseAmount;
        mobAttackAmount = stageData.firstMobAttackAmount;
        mobAttackIncreaseAmount = stageData.mobAttackIncreaseAmount;

        GameStartEvent.Occurred();

        goldValue.RuntimeValue += 20;
        goldChangeEvent.Occurred();

        //TurnStart();
    }

    private void Update()
    {
        
    }



    public void AddScore(int amount)
    {
        goldValue.RuntimeValue += amount;

        goldChangeEvent.Occurred();
    }

    public void RerollScore()
    {
        goldValue.RuntimeValue -= 1 + 1 * rerollCount;

        goldChangeEvent.Occurred();

        rerollCount++;
    }

    //public void OnClickDeckSee()
    //{
    //    isDeckSee = !isDeckSee;
    //    deckSeePanel.gameObject.SetActive(isDeckSee);
    //    player.gameObject.SetActive(!isDeckSee);

    //    if (isDeckSee == true)
    //    {
    //        foreach (RectTransform item in deckSeePanelTrm)
    //        {
    //            Destroy(item.gameObject);
    //        }

    //        monsterInt = 0;
    //        hpInt = 0;

    //        foreach (var item in HandleManager.Instance.deck)
    //        {
    //            Instantiate(seeCardPrefab, deckSeePanelTrm);
    //            CardPower cardPower = seeCardPrefab.GetComponent<CardPower>();
    //            cardPower.cardType = item.cardType;
    //            cardPower.value = item.value;
    //            cardPower.ApplyUI();

                

    //            if (item.cardType == CardType.Monster)
    //            {
    //                monsterInt += item.value;
    //            }

    //            if (item.cardType == CardType.Potion 
    //                || item.cardType == CardType.Sheild 
    //                || item.cardType == CardType.Sword)
    //            {
    //                hpInt += item.value;
    //            }
    //        }

    //        hpIntText.text = $"hp : {hpInt}";
    //        mobIntText.text = $"mob : {monsterInt}";
    //    }
    //}

    public void SetPlayerPos()
    {
        StartCoroutine(TempPlayerPosSetCor());
    }

    private IEnumerator TempPlayerPosSetCor()
    {
        yield return new WaitForSeconds(0.25f);

        Vector3 movePos = MapManager.Instance.fieldList[MapManager.Instance.fieldList.Count - 1].transform.position;
        player.transform.DOMove(movePos, 0.25f);
    }


    public void TurnStart()
    {
        if (moveIndex == 0)
        {
            // 턴 증가
            turnCountValue.RuntimeValue++;

            // 모든 필드의 필드타입 yet으로
            foreach (Field field in MapManager.Instance.fieldList)
            {
                field.fieldType = FieldType.yet;
            }


            // 랜덤 몹 생성
            CreateRandomMob();

            // 앞에 3칸 활성화
            for (int i = 0; i < maxMoveCount; i++) 
            {
                if(MapManager.Instance.fieldList[i].fieldType == FieldType.yet)
                {
                    MapManager.Instance.fieldList[i].fieldType = FieldType.able;

                    MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.nowFieldSprite;
                }
            }

            // change mode 활성화
            //isChange = true;

            // 수치 증가
            mobSpawnAmount += mobSpawnIncreaseAmount;
            mobAttackAmount += mobAttackIncreaseAmount;

            // 카드 뽑기
            TurnStartEvent.Occurred();
        }
    }

    private void CreateRandomMob()
    {
        List<int> canSpawnList = new List<int>();
        List<int> deleteFieldList = new List<int>();

        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
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
            if(canSpawnList.Count <= 0)
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
                for(int j = i; j < mobSpawnAmount; j++)
                {
                    // mobSpawnAmount >= 타일개수면 더이상못깔음, ex) 20개일때 20>=20이면 리스트[20] null

                    if (j >= MapManager.Instance.fieldList.Count)
                    {
                        return;
                    }
                    CreateEnemy(deleteFieldList[j - i]);
                }

                break;
            }

            // 10
            int randIndex = canSpawnList[UnityEngine.Random.Range(0, canSpawnList.Count)];

            // 선택된리스트에 추가
            CreateEnemy(randIndex);

            // 뽑은거 + 뽑은거근처 제거
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
    }

    /// <summary> 맵의 특정 칸에 몬스터를 생성합니다. </summary>
    /// <param name="fieldIndex">생성할 칸</param>
    public void CreateEnemy(int fieldIndex)
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
        MapManager.Instance.fieldList[fieldIndex].Init(cardPower, dragbleCard, FieldType.not);

        // 배경색 변경
        cardPower.backImage.color = Color.magenta;

        // craete effect
        EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
    }

    public void TurnEnd()
    {
        // 이전 4개의 필드
        for (int i = moveIndex - 4; i < moveIndex; i++)
        {
            // (fieldType = not)
            MapManager.Instance.fieldList[i].fieldType = FieldType.not;

            // drag and drop 못하게
            MapManager.Instance.fieldList[i].dragbleCard.canDragAndDrop = false;

            MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.originFieldSprite;
        }

        moveIndex = 0;
        moveCount = 0;
        isMoving = false;


        // 정산
        StartCoroutine(JungSanCor());

        // NextTurnEvent에서 TurnStart를 해준다
    }

    public IEnumerator JungSanCor()
    {
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            Field nowField = MapManager.Instance.fieldList[i];

            CardPower cardPower = nowField.cardPower;
            if (cardPower.cardType == CardType.Monster)
            {
                // 필드 리셋
                MapManager.Instance.fieldList[i].FieldReset();

                // effect
                EffectManager.Instance.GetJungSanEffect(nowField.transform.position);

                // coin 생성
                // GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position);
                GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position);
                yield return new WaitForSeconds(fieldResetDelay);
            }
            else
            {
                // 필드 리셋
                nowField.FieldReset();
            }
        }

        yield return new WaitForSeconds(1f);

        GoldAnimManager.Instance.GetAllCoin();
    }

    public void MoveStart()
    {
        // 카드에 건물 효과 적용
        for (int i = moveIndex; i < moveIndex + 4; i++)
        {
            MapManager.Instance.fieldList[i].OnAccessCard();
        }

        isMoving = true;
    }

    public void MoveEnd()
    {
        moveCount = 0;
        isMoving = false;

        // 사용하지않은 카드 제거

        // 이전 4개의 필드
        for (int i = moveIndex - 4; i < moveIndex; i++)
        {
            // (fieldType = not)
            MapManager.Instance.fieldList[i].fieldType = FieldType.not;

            // drag and drop 못하게
            MapManager.Instance.fieldList[i].dragbleCard.canDragAndDrop = false;

            MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.originFieldSprite;
        }

        // 다음 필드(fieldType 변경)
        for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
        {
            if (MapManager.Instance.fieldList[i].fieldType == FieldType.yet)
            {
                MapManager.Instance.fieldList[i].fieldType = FieldType.able;
            }

            MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.nowFieldSprite;
        }
    }

    public void Move()
    {
        // 움직이기
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            Vector3 movePos = MapManager.Instance.fieldList[moveIndex].transform.position;
            player.transform.DOMove(movePos, 0.25f);
            //player.Move(MapManager.Instance.fieldRectList[moveIndex].transform.position, 0.25f);

        });
        sequence.AppendInterval(0.25f);

        // 스페셜카드 효과 발동
        sequence.AppendCallback(() =>
        {
            if (MapManager.Instance.fieldList[moveIndex].cardPower.cardType != CardType.NULL)
            {
                MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(moveDuration);

        // 플레이어한테 필드 효과 적용ㅇ
        sequence.AppendCallback(() =>
        {
            if (MapManager.Instance.fieldList[moveIndex].cardPower.cardType != CardType.NULL)
            {
                player.OnFeild(MapManager.Instance.fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(moveDuration);

        // 플레이어한테 건물효과 적용
        sequence.AppendCallback(() =>
        {
            MapManager.Instance.fieldList[moveIndex].accessBuildToPlayerAfterOnField?.Invoke(player);
            //Debug.Log("player apply build");
        });
        sequence.AppendInterval(moveDuration);

        sequence.AppendCallback(() => {
            moveIndex++;
            moveCount++;

            NextAction();
        });
    }

    public void NextAction()
    {
        // TurnEnd
        if (moveIndex == MapManager.Instance.fieldList.Count)
        {
            Debug.Log("Loop End");
            TurnEnd();

            TurnEndEvent.Occurred();

            return;
        }

        // move start
        if (moveCount == 0)
        {
            MoveStartEvent.Occurred();

            MoveStart();
        }

        // move end
        if (moveCount == maxMoveCount)
        {
            MoveEnd();

            // 카드 뽑기
            MoveEndEvent.Occurred();

            return;
        }


        Move();
    }

    public void DropByRightClick(DragbleCard dragbleCard)
    {
        int tempIndex = -1; // 4칸중 비어있는 필드의 인덱스를 담을 곳
        for (int i = 0; i < 4; i++)
        {
            if (MapManager.Instance.fieldList[moveIndex + i].dragbleCard == null)
            {
                tempIndex = moveIndex + i;
                break;
            }
        }

        // 비어있는 곳 없으면 리턴
        if (tempIndex == -1)
        {
            return;
        }

        // 부모 설정
        DropArea tempDropArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        dragbleCard.transform.SetParent(tempDropArea.rectTrm, true);

        // 정보 설정
        CardPower cardPower = dragbleCard.GetComponent<CardPower>();

        tempDropArea.field.cardPower = cardPower;
        tempDropArea.field.dragbleCard = dragbleCard;
    }

    public void OnClickMove()
    {
        // move중일때 또 next를 누르지 못하게
        if (isMoving == false)
        {
            for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                // 전부 다 배치안했으면 move 안됨
                if (MapManager.Instance.fieldList[i].cardPower == null)
                {
                    return;
                }
            }

            NextAction();
        }
    }

    public void OnClickSpeedAdd(float amount)
    {
        moveDuration = Mathf.Clamp(moveDuration + amount, 0.01f, 1f);
    }
}    