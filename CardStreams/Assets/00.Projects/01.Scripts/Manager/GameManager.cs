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
    TurnStart,
    TurnEnd,
    Move,
    Modify,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isMoving;  // move���϶� �� next�� ������ ���ϰ�

    [HideInInspector]public int rerollCount;

    public bool canStartTurn;

    [Header("UI")]
    public Player player;

    [Header("System")]
    [SerializeField] float moveDuration;
    [SerializeField] float fieldResetDelay;
    private int maxMoveCount = 3;  // n
    private int moveIndex = 0;
    private GameState curState;

    [Header("StageData")]
    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;

    [Header("Controller")]
    private FieldController fieldController;
    public EnemyController enemyController;
    public HandleController handleController;


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

        //TurnStart();
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

    // To Do : ���߿� ����
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
        // �� ����
        turnCountValue.RuntimeValue++;

        // ��� �ʵ��� �ʵ�Ÿ�� yet����
        fieldController.SetAllFieldYet();
        // �տ� nĭ Ȱ��ȭ
        fieldController.SetNextFieldAble(moveIndex);

        enemyController.CreateRandomMob();
        enemyController.RandomEnemyBuild();

        handleController.DrawBuildAndSpecialWhenTurnStart();
        handleController.DrawCardWhenBeforeMove();

        TurnStartEvent.Occurred();

        curState = GameState.Move;
    }

    public void TurnEnd()
    {
        handleController.TurnEnd();

        moveIndex = 0;
        isMoving = false;

        // ����
        StartCoroutine(JungSanCor());

        // NextTurnEvent���� TurnStart�� ���ش�

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
                // �ʵ� ����
                MapManager.Instance.fieldList[i].FieldReset();

                // effect
                EffectManager.Instance.GetJungSanEffect(nowField.transform.position);

                // coin ����
                // GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position);
                GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position);
                yield return new WaitForSeconds(fieldResetDelay);
            }
            else
            {
                // �ʵ� ����
                nowField.FieldReset();
            }
        }

        yield return new WaitForSeconds(1f);

        GoldAnimManager.Instance.GetAllCoin();
    }

    public void MoveStart()
    {
        // ī�忡 �ǹ� ȿ�� ����
        fieldController.BuildAccessNextField(moveIndex);

        isMoving = true;

        TurnStartEvent.Occurred();
    }

    public void MoveEnd()
    {
        isMoving = false;

        // ����������� ī�� ����

        // ���� 4���� �ʵ�
        fieldController.SetBeforeFieldNot(moveIndex);

        // �������� �ƴ϶�� Ȥ�� ���� ĭ�� �ִٸ�
        if(moveIndex != MapManager.Instance.fieldCount)
        {
            // ���� �ʵ�(fieldType ����)
            fieldController.SetNextFieldAble(moveIndex);
            // draw
            handleController.DrawCardWhenBeforeMove();
        }

        MoveEndEvent.Occurred();
    }

    public void Move()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => MoveStart());

        for(int i = 0; i < maxMoveCount; i++)
        {
            // player ��ġ �̵�
            sequence.AppendCallback(() =>
            {
                Vector3 movePos = MapManager.Instance.fieldList[moveIndex].transform.position;
                player.transform.DOMove(movePos, 0.25f);
                //player.Move(MapManager.Instance.fieldRectList[moveIndex].transform.position, 0.25f);

            });
            sequence.AppendInterval(0.25f);

            // �����ī�� ȿ�� �ߵ�
            sequence.AppendCallback(() =>
            {
                if (MapManager.Instance.fieldList[moveIndex].cardPower.cardType != CardType.NULL)
                {
                    MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
                }
            });
            sequence.AppendInterval(moveDuration);

            // �÷��̾����� �ʵ� ȿ�� ����
            sequence.AppendCallback(() =>
            {
                if (MapManager.Instance.fieldList[moveIndex].cardPower.cardType != CardType.NULL)
                {
                    player.OnFeild(MapManager.Instance.fieldList[moveIndex]);
                }
            });
            sequence.AppendInterval(moveDuration);

            // �÷��̾����� �ǹ�ȿ�� ����
            sequence.AppendCallback(() =>
            {
                MapManager.Instance.fieldList[moveIndex].accessBuildToPlayerAfterOnField?.Invoke(player);
                //Debug.Log("player apply build");
            });
            sequence.AppendInterval(moveDuration);

            sequence.AppendCallback(() => {
                moveIndex++;

                // �÷��̾� �׾����� ��
                if (player.isAlive == false)
                {
                    Debug.Log("�÷��̾� ����");
                    playerDieEvent.Occurred();
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

                NextAction();
            }
        });
    }

    public void NextAction()
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

                break;
            default:
                break;
        }
    }

    public bool DropByRightClick(DragbleCard dragbleCard)
    {
        int tempIndex = -1; // 4ĭ�� ����ִ� �ʵ��� �ε����� ���� ��
        for (int i = 0; i < 4; i++)
        {
            if (MapManager.Instance.fieldList[moveIndex + i].dragbleCard == null)
            {
                tempIndex = moveIndex + i;
                break;
            }
        }

        // ����ִ� �� ������ ����
        if (tempIndex == -1)
        {
            return false;
        }


        DropArea dropArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        // drop area ����
        dragbleCard.SetDroppedArea(dropArea);

        // �θ� ����
        dragbleCard.transform.SetParent(dropArea.rectTrm, true);

        dragbleCard.IsField();

        // ���� ����
        CardPower cardPower = dragbleCard.GetComponent<CardPower>();

        dropArea.field.cardPower = cardPower;
        dropArea.field.dragbleCard = dragbleCard;

        return true;
    }

    public void OnClickMove()
    {
        bool canNextAction = true;

        // move���϶� �� next�� ������ ���ϰ�
        if (curState == GameState.Move && isMoving == false)
        {
            for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                // ���� �� ��ġ�������� move �ȵ�
                if (MapManager.Instance.fieldList[i].cardPower == null)
                {
                    canNextAction = false;
                }
            }
        }

        if(curState == GameState.Modify)
        {
            curState = GameState.TurnStart;
        }

        if(canNextAction == true)
        {
            NextAction();
        }
    }

    public void OnClickSpeedAdd(float amount)
    {
        moveDuration = Mathf.Clamp(moveDuration + amount, 0.01f, 1f);
    }
}    