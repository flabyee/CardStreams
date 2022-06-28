using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public enum GameState
{
    NULL,
    TurnStart,
    TurnEnd,
    Move,
    Modify,
    Equip,
    GameStart,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [HideInInspector]public int rerollCount;


    [Header("Player")]
    public Player player;

    [Header("System")]
    [SerializeField] float moveDuration;    // Move에 걸리는 시간
    [SerializeField] float fieldResetDelay; // 정산이후 대기 시간
    public bool canStartTurn;   // deck을 다 만든 후에 게임을 진행하기 위해서
    public int moveIndex = 0;  // 현재 플레이어가 맵에 위치한 곳
    public GameState curState; // 현재 게임의 상태
    public GameState nextState; // 다음 상태
    public bool canNext;

    // 임시
    private bool isFirst = false;
    private int bossRound;

    [Header("StageData")]
    private int maxMoveCount;



    [Header("Controller")]
    public FieldController fieldController;
    public EnemyController enemyController;
    public HandleController handleController;
    public ShopController shopController;
    public SelectRewardManager selectRewardManager;
    public DontTouchController dontTouchController;
    public BlurCoverController blurController;


    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue loopCountValue;
    

    [Header("Event")]
    public EventSO TurnStartEvent;
    public EventSO TurnEndEvent;
    public EventSO MoveStartEvent;
    public EventSO MoveEndEvent;

    public EventSO goldChangeEvent;
    public EventSO playerDieEvent;
    public EventSO loopChangeEvent;

    public Action<int> ShowTuTorialEvent;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("init");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (DataManager.Instance.stageNumValue.RuntimeValue == 0)
        {
            curState = GameState.GameStart;
            nextState = GameState.TurnStart;
        }
        else
        {
            curState = GameState.GameStart;
            nextState = GameState.Modify;
        }

        isFirst = false;
        canNext = false;

        LoadStageData();

        fieldController = new FieldController(maxMoveCount);
        fieldController.SetAllFieldYet();

        goldValue.RuntimeValue += 20;
        goldChangeEvent.Occurred();

        loopCountValue.RuntimeValue = 0;
        loopChangeEvent.Occurred();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopGame();
        }
    }

    public void LoadStageData()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();
        maxMoveCount = stageData.moveCount;
        bossRound = stageData.bossRound;
    }

    // To Do : 나중에 수정
    public void SetPlayerPos()
    {
        StartCoroutine(TempPlayerPosSetCor());
    }

    private IEnumerator TempPlayerPosSetCor()
    {
        yield return new WaitForSeconds(0.25f);

        Vector3 movePos = MapManager.Instance.fieldList[MapManager.Instance.fieldCount - 1].transform.position;
        player.transform.DOMove(movePos, 0.25f);
    }

    public void NextAction()
    {
        switch (nextState)
        {
            case GameState.TurnStart:
                curState = GameState.TurnStart;
                TurnStart();
                break;
            case GameState.TurnEnd:
                curState = GameState.TurnEnd;
                TurnEnd();
                break;
            case GameState.Move:
                curState = GameState.Move;
                Move();
                break;
            case GameState.Modify:
                curState = GameState.Modify;
                OnModify();
                break;
            case GameState.Equip:
                curState = GameState.Equip;
                OnEquip();
                break;
            default:
                Debug.LogError("ereoroeroeoroeorooeoroeoroeor");
                break;
        }
    }

    public void OnClickAction()
    {
        


        switch (curState)
        {
            case GameState.TurnStart:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("뭘까용~", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;


            case GameState.TurnEnd:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("정산중입니다", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;


            case GameState.Move:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("이동중입니다", new UITooltip.TooltipTimer(1f));
                    return;
                }

                
                // 꽉 차있는지
                if (fieldController.IsNextFieldFull(moveIndex) == false)
                {
                    UITooltip.Instance.Show("앞에 4칸을 전부 채운후 다시 시도하세요!!", new UITooltip.TooltipTimer(1f));
                    return;
                }

                // 플레이어 카드가 2장 이하인지
                if (fieldController.IsNextFieldPlayerCardTwo(moveIndex) == false)
                {
                    UITooltip.Instance.Show("플레이어 카드를 2장 이하로 배치해주세요", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;


            case GameState.Modify:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("무작위 적이 생성중입니다", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;


            case GameState.Equip:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("뭘까용~", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;
            case GameState.GameStart:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("먼저 시작 보상을 선택해주세요", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;
            default:
                break;
        }
    }

    public void TurnStart()
    {
        Debug.Log("start");
        // fieldController.SetAllFieldYet(); // 안해주면 카드가 안들어감 근데 여기다가 이거쓰면 손에보라색몬스터 버그남 ??그럼어떻게해야하지


        // 턴 증가
        loopCountValue.RuntimeValue++;
        loopChangeEvent.Occurred();


        if(loopCountValue.RuntimeValue == 6)
        {
            // 플레이해주셔서 감사합니다
        }


            // 앞에 n칸 활성화
            fieldController.SetNextFieldAble(moveIndex);

        handleController.HandleReturnToDeck();
        handleController.DrawCardWhenBeforeMove();


        BuildManager.Instance.NextBuildEffect();

        TurnStartEvent.Occurred();

        canNext = true;

        ShowTuTorialEvent?.Invoke(1);

        curState = GameState.Move;
        nextState = GameState.Move;
    }

    public void TurnEnd()
    {
        canNext = false;

        if(loopCountValue.RuntimeValue == bossRound - 1)
        {
            handleController.LoopEnd(true);
        }
        else
        {
            handleController.LoopEnd(false);
        }

        moveIndex = 0;
        ShowTuTorialEvent?.Invoke(3);

        // 정산
        StartCoroutine(JungSanCor());

        EffectManager.Instance.DeleteNextBuildEffect();

        TurnEndEvent.Occurred();

        nextState = GameState.Modify;
    }

    public IEnumerator JungSanCor()
    {
        for (int i = 0; i < MapManager.Instance.fieldCount; i++)
        {
            Field nowField = MapManager.Instance.fieldList[i];

            if(nowField.isSet == true)
            {
                BasicCard cardPower = nowField.cardPower as BasicCard;
                if (cardPower.basicType == BasicType.Monster)
                {
                    // 필드 리셋
                    MapManager.Instance.fieldList[i].FieldReset();

                    // effect
                    Effects.Instance.TriggerBlock(nowField.transform.position);

                    GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position, false);

                    yield return new WaitForSeconds(fieldResetDelay);
                }
                else
                {
                    // 필드 리셋
                    nowField.FieldReset();
                }
            }
        }

        yield return new WaitForSeconds(1f);

        GoldAnimManager.Instance.GetAllCoin(true);

        fieldController.SetAllFieldYet();

        canNext = true;
    }

    public void MoveStart()
    {
        handleController.HandleReturnToDeck();

        // 카드에 건물 효과 적용
        fieldController.BuildAccessNextField(moveIndex);

        canNext = false;

        TurnStartEvent.Occurred();

        dontTouchController.Show();
    }

    public void MoveEnd()
    {
        if (moveIndex == maxMoveCount) ShowTuTorialEvent?.Invoke(2);

        canNext = true;

        // 이전 4개의 필드
        fieldController.SetBeforeFieldNot(moveIndex);

        // 마지막이 아니라면 혹은 다음 칸이 있다면
        if(moveIndex != MapManager.Instance.fieldCount)
        {
            // 다음 필드(fieldType 변경)
            fieldController.SetNextFieldAble(moveIndex);
            // draw
            handleController.DrawCardWhenBeforeMove();

            BuildManager.Instance.NextBuildEffect();
        }

        MoveEndEvent.Occurred();

        dontTouchController.Hide();
    }

    public void Move()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => MoveStart());
        sequence.AppendInterval(moveDuration * 3);

        for (int i = 0; i < maxMoveCount; i++)
        {
            // player 위치 이동
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
                if (MapManager.Instance.fieldList[moveIndex].isSet == true)
                {
                    MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
                }
            });
            sequence.AppendInterval(moveDuration);

            // 플레이어한테 필드 효과 적용
            sequence.AppendCallback(() =>
            {
                if (MapManager.Instance.fieldList[moveIndex].isSet == true)
                {
                    player.OnFeild(MapManager.Instance.fieldList[moveIndex]);
                }
            });
            sequence.AppendInterval(moveDuration);

            // 플레이어한테 건물효과 적용
            sequence.AppendCallback(() =>
            {
                // 플레이어에게 적용하는 것으로 cardPower 존재 유무는 상관없다
                MapManager.Instance.fieldList[moveIndex].accessBuildToPlayerAfterOnField?.Invoke(player);
                //Debug.Log("player apply build");
            });
            sequence.AppendInterval(moveDuration);

            sequence.AppendCallback(() => {
                moveIndex++;

                // 플레이어 죽었으면 끝
                if (player.isAlive == false)
                {
                    GameEnd();

                    sequence.Kill();
                    return;
                }
            });
            sequence.AppendInterval(moveDuration);
        }

        sequence.AppendCallback(() => MoveEnd());

        sequence.AppendCallback(() =>
        {
            if (moveIndex == MapManager.Instance.fieldCount)
            {
                nextState = GameState.TurnEnd;

                NextAction();
            }
        });
    }

    private void OnModify()
    {
        canNext = false;

        if(isFirst == true)
        {
            nextState = GameState.Equip;
        }
        else
        {
            nextState = GameState.TurnStart;
        }


        StartCoroutine(Delay(() =>
        {
            if(isFirst == true)
            {
                shopController.Show();
                selectRewardManager.Show();
                blurController.SetActive(true);
            }
            else
            {
                isFirst = true;
            }

            canNext = true;
        }, 1.5f));

        if (loopCountValue.RuntimeValue == bossRound - 1)
        {
            enemyController.BossRound();
        }
        else if(loopCountValue.RuntimeValue < bossRound - 1)
        {
            enemyController.CreateRandomMob();
            enemyController.RandomEnemyBuild();
        }
        else
        {
            Debug.LogError("클리어");
        }
    }

    private void OnEquip()
    {
        shopController.Hide();
        selectRewardManager.Hide();
        blurController.SetActive(false);

        handleController.DrawBuildCard();

        nextState = GameState.TurnStart;
    }

    public bool DropField(DragbleCard dragbleCard)
    {
        int tempIndex = -1; // 4칸중 비어있는 필드의 인덱스를 담을 곳
        for (int i = 0; i < 4; i++)
        {
            if (MapManager.Instance.fieldList[moveIndex + i].isSet == false)
            {
                tempIndex = moveIndex + i;
                break;
            }
        }

        // 비어있는 곳 없으면 리턴
        if (tempIndex == -1)
        {
            return false;
        }

        DropArea handleArea = dragbleCard.originDropArea;
        handleArea.TriggerOnLift(dragbleCard);

        DropArea dropArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        dropArea.TriggerOnDrop(dragbleCard);

        dragbleCard.transform.rotation = Quaternion.identity;

        handleController.cardSorting.AlignCards();

        HandleCardTooltip.Instance.Hide();

        return true;
    }

    public void StopGame()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SetMoveDuration(float speed)
    {
        moveDuration = speed;
    }

    private IEnumerator Delay(Action action, float t)
    {
        yield return new WaitForSeconds(t);

        action?.Invoke();
    }

    private void GameEnd()
    {
        SaveData saveData = SaveSystem.Load();
        saveData.gold += player.killMobCount;
        SaveSystem.Save(saveData);

        Debug.Log("플레이어 디짐");
        dontTouchController.Hide();
        playerDieEvent.Occurred();
        DropArea.dropAreas.Clear();
        DontRaycastTarget.dontRaycastTargetList.Clear();
    }
} 