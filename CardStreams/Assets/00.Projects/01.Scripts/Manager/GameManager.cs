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


    [Header("System")]
    private bool isMoving;  // move중일때 또 next를 누르지 못하게
    [SerializeField] float moveDuration;
    [SerializeField] float lastDuration;

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
    public EventSO FieldResetAfter;
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
        // 턴시작시 몬스터? 설치불가타일? 생성
        bool[] isMonster = new bool[30];
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            if (i < mobSpawnAmount)
                isMonster[i] = true;
            else
                isMonster[i] = false;
        }
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            int j = UnityEngine.Random.Range(0, MapManager.Instance.fieldList.Count);
            bool temp = isMonster[i];
            isMonster[i] = isMonster[j];
            isMonster[j] = temp;
        }
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            if (isMonster[i] == true)
            {
                int value = mobAttackAmount; // 생성되는 몬스터의 값

                // 새로운 카드 생성
                GameObject cardObj = Instantiate(cardPrefab, MapManager.Instance.fieldList[i].transform);
                DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
                CardPower cardPower = cardObj.GetComponent<CardPower>();

                // cardPower에 정보 넣기
                dragbleCard.SetData_Feild(CardType.Monster, value);

                // 못 움직이게
                dragbleCard.canDragAndDrop = false;

                // 필드에 적용 + not으로
                MapManager.Instance.fieldList[i].cardPower = cardPower;
                MapManager.Instance.fieldList[i].dragbleCard = dragbleCard;
                MapManager.Instance.fieldList[i].fieldType = FieldType.not;

                // 배경색 변경
                cardPower.backImage.color = Color.magenta;

                // craete effect
                EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[i].transform.position);
            }
        }
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
                GoldAnimManager.Instance.CreateCoin(cardPower.value, nowField.transform.position);

                yield return new WaitForSeconds(0.25f);
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

    public void OnClickMove()
    {
        // move중일때 또 next를 누르지 못하게
        if (isMoving == false)
        {
            for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                // 전부 다 배치안했으면 move 안됨
                if(MapManager.Instance.fieldList[i].cardPower == null)
                {
                    return;
                }
            }

            NextAction();
        }
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
}    