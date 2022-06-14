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
    private bool canNext;

    [Header("StageData")]
    private int maxMoveCount;

    [Header("Controller")]
    private FieldController fieldController;
    public EnemyController enemyController;
    public HandleController handleController;
    public ShopController shopController;
    public SelectRewardManager selectRewardManager;
    public DontTouchController dontTouchController;


    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue turnCountValue;
    

    [Header("Event")]
    public EventSO TurnStartEvent;
    public EventSO TurnEndEvent;
    public EventSO MoveStartEvent;
    public EventSO MoveEndEvent;

    public EventSO goldChangeEvent;
    public EventSO playerDieEvent;

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

        curState = GameState.TurnEnd;
        nextState = GameState.Modify;
        canNext = true;
    }

    private void Start()
    {
        LoadStageData();

        fieldController = new FieldController(maxMoveCount);

        goldValue.RuntimeValue += 20;
        goldChangeEvent.Occurred();
    }

    public void LoadStageData()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();
        maxMoveCount = stageData.moveCount;
    }

    public void AddGold(int amount)
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

                // 손에 몬스터 카드가 있고 앞에 4개 필드가 모두 몬스터가 아니면 next 안됨
                if (handleController.HaveMobCard() == true)
                {
                    if(fieldController.IsNextFieldAllMob(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("모든 몬스터를 배치한 후에 다시 시도하세요!", new UITooltip.TooltipTimer(1f));
                        return;
                    }
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
            default:
                break;
        }
    }

    public void TurnStart()
    {
        Debug.Log("start");

        // 턴 증가
        turnCountValue.RuntimeValue++;


        // 앞에 n칸 활성화
        fieldController.SetNextFieldAble(moveIndex);

        handleController.DrawCardWhenBeforeMove();
        handleController.ShowBuildHandle(false);


        BuildManager.Instance.NextBuildEffect();

        TurnStartEvent.Occurred();

        canNext = true;

        curState = GameState.Move;
        nextState = GameState.Move;
    }

    public void TurnEnd()
    {
        canNext = false;

        handleController.LoopEnd();

        moveIndex = 0;

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
        }

        yield return new WaitForSeconds(1.5f);

        GoldAnimManager.Instance.GetAllCoin(true);

        canNext = true;
    }

    public void MoveStart()
    {
        handleController.SellHandleCards();

        // 카드에 건물 효과 적용
        fieldController.BuildAccessNextField(moveIndex);

        canNext = false;

        TurnStartEvent.Occurred();

        dontTouchController.Show();
    }

    public void MoveEnd()
    {
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
        MoveStart();

        Sequence sequence = DOTween.Sequence();

        for(int i = 0; i < maxMoveCount; i++)
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

        StartCoroutine(Delay(() =>
        {
            shopController.Show();
            selectRewardManager.Show();

            canNext = true;
        }, 1.5f));


        // 모든 필드의 필드타입 yet으로
        fieldController.SetAllFieldYet();

        enemyController.CreateRandomMob();
        enemyController.RandomEnemyBuild();

        handleController.ShowBuildHandle(true);
        handleController.InteractiveBuildHandle(false);

        nextState = GameState.Equip;
    }

    private void OnEquip()
    {
        handleController.ShowBuildHandle(true);
        handleController.InteractiveBuildHandle(true);

        shopController.Hide();
        selectRewardManager.Hide();

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

        DropArea dropArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        // drop area 설정
        dragbleCard.SetDroppedArea(dropArea);

        dropArea.TriggerOnDrop(dragbleCard);

        return true;
    }
    public bool DropQuickSlot(DragbleCard dragbleCard)
    {
        DropArea dropArea = handleController.GetTempQuicSlot();
        // 비어있는 곳 없으면 리턴
        if (dropArea == null)
        {
            return false;
        }

        // drop area 설정
        dragbleCard.SetDroppedArea(dropArea);

        dropArea.TriggerOnDrop(dragbleCard);

        return true;
    }

    public void OnClickSpeedAdd(float amount)
    {
        moveDuration = Mathf.Clamp(moveDuration + amount, 0.01f, 1f);
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
        Debug.Log("플레이어 디짐");
        dontTouchController.Hide();
        playerDieEvent.Occurred();
        DropArea.dropAreas.Clear();
    }
} 