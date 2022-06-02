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
    private bool canMove;      // move중일때 또 next를 누르지 못하게
    private int moveIndex = 0;  // 현재 플레이어가 맵에 위치한 곳
    private GameState curState; // 현재 게임의 상태

    [Header("StageData")]
    private int maxMoveCount;

    [Header("Controller")]
    private FieldController fieldController;
    public EnemyController enemyController;
    public HandleController handleController;
    public ShopController shopController;
    public SelectRewardManager selectRewardManager;


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
        }
        else
        {
            Destroy(this.gameObject);
        }

        curState = GameState.TurnStart;
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


    public void TurnStart()
    {
        // 턴 증가
        turnCountValue.RuntimeValue++;

        // 모든 필드의 필드타입 yet으로
        fieldController.SetAllFieldYet();
        // 앞에 n칸 활성화
        fieldController.SetNextFieldAble(moveIndex);

        enemyController.RandomEnemyBuild();

        handleController.DrawBuildAndSpecialWhenTurnStart();
        handleController.DrawCardWhenBeforeMove();

        shopController.Hide();
        selectRewardManager.Hide();

        TurnStartEvent.Occurred();

        canMove = true;

        curState = GameState.Move;
    }

    public void TurnEnd()
    {
        handleController.TurnEnd();

        moveIndex = 0;

        // 정산
        StartCoroutine(JungSanCor());

        TurnEndEvent.Occurred();

        curState = GameState.Modify;
    }

    public IEnumerator JungSanCor()
    {
        for (int i = 0; i < MapManager.Instance.fieldCount; i++)
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
        fieldController.BuildAccessNextField(moveIndex);

        canMove = false;

        TurnStartEvent.Occurred();
    }

    public void MoveEnd()
    {
        canMove = true;

        // 이전 4개의 필드
        fieldController.SetBeforeFieldNot(moveIndex);

        // 마지막이 아니라면 혹은 다음 칸이 있다면
        if(moveIndex != MapManager.Instance.fieldCount)
        {
            // 다음 필드(fieldType 변경)
            fieldController.SetNextFieldAble(moveIndex);
            // draw
            handleController.DrawCardWhenBeforeMove();
        }

        MoveEndEvent.Occurred();
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
                if (MapManager.Instance.fieldList[moveIndex].cardPower.cardType != CardType.NULL)
                {
                    MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
                }
            });
            sequence.AppendInterval(moveDuration);

            // 플레이어한테 필드 효과 적용
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

                // 플레이어 죽었으면 끝
                if (player.isAlive == false)
                {
                    Debug.Log("플레이어 디짐");
                    playerDieEvent.Occurred();
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
                curState = GameState.TurnEnd;

                Action();
            }
        });
    }

    private void OnModify()
    {
        shopController.Show();
        selectRewardManager.Show();

        enemyController.CreateRandomMob();

        curState = GameState.TurnStart;
    }
    private void OffModify()
    {
        shopController.Hide();
    }

    public void Action()
    {
        switch (curState)
        {
            case GameState.TurnStart:
                TurnStart();
                break;
            case GameState.TurnEnd:
                TurnEnd();
                break;
            case GameState.Move:
                Move();
                break;
            case GameState.Modify:
                OnModify();
                break;
            default:
                Debug.LogError("ereoroeroeoroeorooeoroeoroeor");
                break;
        }
    }

    public bool DropByRightClick(DragbleCard dragbleCard)
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
            return false;
        }

        DropArea dropArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        // drop area 설정
        dragbleCard.SetDroppedArea(dropArea);

        // 부모 설정
        dragbleCard.transform.SetParent(dropArea.rectTrm, true);

        dragbleCard.IsField();

        // 정보 설정
        CardPower cardPower = dragbleCard.GetComponent<CardPower>();

        dropArea.field.cardPower = cardPower;
        dropArea.field.dragbleCard = dragbleCard;

        return true;
    }

    public void OnClickAction()
    {
        switch (curState)
        {
            case GameState.TurnStart:
                Action();
                break;


            case GameState.TurnEnd:
                Action();
                break;


            case GameState.Move:
                bool isBreak = false;

                if (canMove == true)
                {
                    for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
                    {
                        // 전부 다 배치안했으면 move 안됨
                        if (MapManager.Instance.fieldList[i].cardPower == null)
                        {
                            isBreak = true;
                        }
                    }
                }
                else
                {
                    isBreak = true;
                }

                if (isBreak)
                    break;

                Action();
                break;


            case GameState.Modify:
                Action();
                break;
            default:
                break;
        }
    }

    public void OnClickSpeedAdd(float amount)
    {
        moveDuration = Mathf.Clamp(moveDuration + amount, 0.01f, 1f);
    }
} 