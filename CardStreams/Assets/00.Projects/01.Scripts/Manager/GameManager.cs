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
    public int moveIndex = 0;  // ���� �÷��̾ �ʿ� ��ġ�� ��
    public GameState curState; // ���� ������ ����
    public GameState nextState; // ���� ����
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
                    UITooltip.Instance.Show("�����~", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;


            case GameState.TurnEnd:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("�������Դϴ�", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;


            case GameState.Move:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("�̵����Դϴ�", new UITooltip.TooltipTimer(1f));
                    return;
                }

                // �տ� ���� ī�尡 �ְ� �տ� 4�� �ʵ尡 ��� ���Ͱ� �ƴϸ� next �ȵ�
                if (handleController.HaveMobCard() == true)
                {
                    if(fieldController.IsNextFieldAllMob(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("��� ���͸� ��ġ�� �Ŀ� �ٽ� �õ��ϼ���!", new UITooltip.TooltipTimer(1f));
                        return;
                    }
                }

                NextAction();
                break;


            case GameState.Modify:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("������ ���� �������Դϴ�", new UITooltip.TooltipTimer(1f));
                    return;
                }

                NextAction();
                break;


            case GameState.Equip:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("�����~", new UITooltip.TooltipTimer(1f));
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

        // �� ����
        turnCountValue.RuntimeValue++;


        // �տ� nĭ Ȱ��ȭ
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

        // ����
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
                    // �ʵ� ����
                    MapManager.Instance.fieldList[i].FieldReset();

                    // effect
                    Effects.Instance.TriggerBlock(nowField.transform.position);

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
        }

        yield return new WaitForSeconds(1.5f);

        GoldAnimManager.Instance.GetAllCoin(true);

        canNext = true;
    }

    public void MoveStart()
    {
        handleController.SellHandleCards();

        // ī�忡 �ǹ� ȿ�� ����
        fieldController.BuildAccessNextField(moveIndex);

        canNext = false;

        TurnStartEvent.Occurred();

        dontTouchController.Show();
    }

    public void MoveEnd()
    {
        canNext = true;

        // ���� 4���� �ʵ�
        fieldController.SetBeforeFieldNot(moveIndex);

        // �������� �ƴ϶�� Ȥ�� ���� ĭ�� �ִٸ�
        if(moveIndex != MapManager.Instance.fieldCount)
        {
            // ���� �ʵ�(fieldType ����)
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
                if (MapManager.Instance.fieldList[moveIndex].isSet == true)
                {
                    MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
                }
            });
            sequence.AppendInterval(moveDuration);

            // �÷��̾����� �ʵ� ȿ�� ����
            sequence.AppendCallback(() =>
            {
                if (MapManager.Instance.fieldList[moveIndex].isSet == true)
                {
                    player.OnFeild(MapManager.Instance.fieldList[moveIndex]);
                }
            });
            sequence.AppendInterval(moveDuration);

            // �÷��̾����� �ǹ�ȿ�� ����
            sequence.AppendCallback(() =>
            {
                // �÷��̾�� �����ϴ� ������ cardPower ���� ������ �������
                MapManager.Instance.fieldList[moveIndex].accessBuildToPlayerAfterOnField?.Invoke(player);
                //Debug.Log("player apply build");
            });
            sequence.AppendInterval(moveDuration);

            sequence.AppendCallback(() => {
                moveIndex++;

                // �÷��̾� �׾����� ��
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


        // ��� �ʵ��� �ʵ�Ÿ�� yet����
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
        int tempIndex = -1; // 4ĭ�� ����ִ� �ʵ��� �ε����� ���� ��
        for (int i = 0; i < 4; i++)
        {
            if (MapManager.Instance.fieldList[moveIndex + i].isSet == false)
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
        Debug.Log("�÷��̾� ����");
        dontTouchController.Hide();
        playerDieEvent.Occurred();
        DropArea.dropAreas.Clear();
    }
} 