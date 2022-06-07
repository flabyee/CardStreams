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
    [SerializeField] float moveDuration;    // Move�� �ɸ��� �ð�
    [SerializeField] float fieldResetDelay; // �������� ��� �ð�
    public bool canStartTurn;   // deck�� �� ���� �Ŀ� ������ �����ϱ� ���ؼ�
    private bool canMove;      // move���϶� �� next�� ������ ���ϰ�
    private int moveIndex = 0;  // ���� �÷��̾ �ʿ� ��ġ�� ��
    public GameState curState; // ���� ������ ����
    public GameState nextState; // ���� ����

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

        curState = GameState.Modify;
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

        enemyController.RandomEnemyBuild();

        handleController.DrawCardWhenBeforeMove();
        handleController.ShowBuildHandle(false);



        TurnStartEvent.Occurred();

        canMove = true;

        curState = GameState.Move;
    }

    public void TurnEnd()
    {
        handleController.TurnEnd();

        moveIndex = 0;

        // ����
        StartCoroutine(JungSanCor());

        TurnEndEvent.Occurred();

        curState = GameState.Modify;
    }

    public IEnumerator JungSanCor()
    {
        for (int i = 0; i < MapManager.Instance.fieldCount; i++)
        {
            Field nowField = MapManager.Instance.fieldList[i];

            BasicCard cardPower = nowField.cardPower as BasicCard;
            if (cardPower.basicType == BasicType.Monster)
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

        yield return new WaitForSeconds(1.5f);

        GoldAnimManager.Instance.GetAllCoin();
    }

    public void MoveStart()
    {
        // ī�忡 �ǹ� ȿ�� ����
        fieldController.BuildAccessNextField(moveIndex);

        canMove = false;

        TurnStartEvent.Occurred();
    }

    public void MoveEnd()
    {
        canMove = true;

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
        MoveStart();

        Sequence sequence = DOTween.Sequence();

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

                NextAction();
            }
        });
    }

    private void OnModify()
    {
        shopController.Show();
        selectRewardManager.Show();

        enemyController.CreateRandomMob();

        handleController.ShowBuildHandle(true);

        curState = GameState.Equip;
    }

    private void OnEquip()
    {
        handleController.ShowBuildHandle(true);
        handleController.DrawBuildAndSpecialWhenTurnStart();

        shopController.Hide();
        selectRewardManager.Hide();

        curState = GameState.TurnStart;
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
                OnModify();
                break;
            case GameState.Equip:
                OnEquip();
                break;
            default:
                Debug.LogError("ereoroeroeoroeorooeoroeoroeor");
                break;
        }
    }

    public bool DropField(DragbleCard dragbleCard)
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

        dropArea.TriggerOnDrop(dragbleCard);

        return true;
    }
    public bool DropQuickSlot(DragbleCard dragbleCard)
    {
        DropArea dropArea = handleController.GetTempQuicSlot();
        // ����ִ� �� ������ ����
        if (dropArea == null)
        {
            return false;
        }

        // drop area ����
        dragbleCard.SetDroppedArea(dropArea);

        dropArea.TriggerOnDrop(dragbleCard);

        return true;
    }

    public void OnClickAction()
    {
        switch (curState)
        {
            case GameState.TurnStart:
                NextAction();
                break;


            case GameState.TurnEnd:
                NextAction();
                break;


            case GameState.Move:
                bool isBreak = false;

                if (canMove == true)
                {
                    for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
                    {
                        // ���� �� ��ġ�������� move �ȵ�
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

                NextAction();
                break;


            case GameState.Modify:
                NextAction();
                break;
            case GameState.Equip:
                NextAction();
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