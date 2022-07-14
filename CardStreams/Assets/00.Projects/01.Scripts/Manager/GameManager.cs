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
    TutoEnd,
    GameEnd,
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
    public bool canNext;

    // �ӽ�
    private bool isFirst = false;
    private bool isTutoEnd = false;
    private bool isBossEnd = false;
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

    [Header("UI")]
    public Text nextStateText;
    public Text curStateText;

    // �ӽ� ���߿� �ٸ� ������ �̵�
    public GameObject tutoEndPanel;
    public GameObject clearPanel;

    public Image stopImage;
    public Sprite playSprite;
    public Sprite stopSprite;

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
            canNext = true;
        }
        else
        {
            curState = GameState.GameStart;
            nextState = GameState.Modify;
            canNext = false;
        }

        ApplyStateText();

        // isFirst �ݴ�� �Ǿ��ִ� ��?
        isFirst = true;
        

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
            case GameState.TutoEnd:
                dontTouchController.Hide();
                DropArea.dropAreas.Clear();
                DontRaycastTarget.dontRaycastTargetList.Clear();
                ShowTuTorialEvent?.Invoke(3);
                break;
            case GameState.GameEnd:
                clearPanel.SetActive(true);
                break;
            default:
                Debug.LogError("ereoroeroeoroeorooeoroeoroeor");
                break;
        }

        ApplyStateText();
    }

    public void OnClickAction()
    {
        SoundManager.Instance.PlaySFX(SFXType.NextButton);

        switch (curState)
        {
            case GameState.TurnStart:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("�����~", new UITooltip.TooltipTimer(1f));
                    return;
                }

                // �� ���ִ���
                if (fieldController.IsNextFieldFull(moveIndex) == false)
                {
                    UITooltip.Instance.Show("�տ� 4ĭ�� ���� ä���� �ٽ� �õ��ϼ���!!", new UITooltip.TooltipTimer(1f));
                    return;
                }

                // �÷��̾� ī�尡 2�� ��������
                if (fieldController.IsNextFieldPlayerCardTwo(moveIndex) == false)
                {
                    UITooltip.Instance.Show("�÷��̾� ī�带 2�� ���Ϸ� ��ġ���ּ���", new UITooltip.TooltipTimer(1f));
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

                
                // �� ���ִ���
                if (fieldController.IsNextFieldFull(moveIndex) == false)
                {
                    UITooltip.Instance.Show("�տ� 4ĭ�� ���� ä���� �ٽ� �õ��ϼ���!!", new UITooltip.TooltipTimer(1f));
                    return;
                }

                // �÷��̾� ī�尡 2�� ��������
                if (fieldController.IsNextFieldPlayerCardTwo(moveIndex) == false)
                {
                    UITooltip.Instance.Show("�÷��̾� ī�带 2�� ���Ϸ� ��ġ���ּ���", new UITooltip.TooltipTimer(1f));
                    return;
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
            case GameState.GameStart:
                if (canNext == false)
                {
                    UITooltip.Instance.Show("���� ���� ������ �������ּ���", new UITooltip.TooltipTimer(1f));
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
        // fieldController.SetAllFieldYet(); // �����ָ� ī�尡 �ȵ� �ٵ� ����ٰ� �̰ž��� �տ���������� ���׳� ??�׷�����ؾ�����


        // �� ����
        loopCountValue.RuntimeValue++;
        loopChangeEvent.Occurred();


        if(loopCountValue.RuntimeValue == 6)
        {
            // �÷������ּż� �����մϴ�
        }


            // �տ� nĭ Ȱ��ȭ
        fieldController.SetNextFieldAble(moveIndex);

        handleController.HandleReturnToDeck();
        handleController.DrawCardWhenBeforeMove();
        handleController.notHaveBuildUI.SetActive(false);


        BuildManager.Instance.NextBuildEffect();

        TurnStartEvent.Occurred();

        canNext = true;

        if(isFirst == true)
            ShowTuTorialEvent?.Invoke(1);

        curState = GameState.TurnStart;
        nextState = GameState.Move;
    }

    public void TurnEnd()
    {
        if(isBossEnd == true)
        {
            blurController.SetActive(true);
            clearPanel.SetActive(true);
            canNext = false;
            return;
        }

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

        // ����
        StartCoroutine(JungSanCor());

        EffectManager.Instance.DeleteNextBuildEffect();

        TurnEndEvent.Occurred();

        nextState = GameState.Modify;

        if(isTutoEnd == true)
        {
            nextState = GameState.TutoEnd;
        }
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

                    GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position, false);

                    yield return new WaitForSeconds(fieldResetDelay);
                }
                else
                {
                    // �ʵ� ����
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

        // ī�忡 �ǹ� ȿ�� ����
        //fieldController.BuildAccessNextField(moveIndex);

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
        Sequence sequence = DOTween.Sequence();

        canNext = false;

        sequence.AppendCallback(() => MoveStart());
        sequence.AppendInterval(moveDuration * 3);

        for (int i = 0; i < maxMoveCount; i++)
        {
            SoundManager.Instance.PlaySFX(SFXType.Moving, 1.5f);

            // player ��ġ �̵�
            sequence.AppendCallback(() =>
            {
                Vector3 movePos = MapManager.Instance.fieldList[moveIndex].transform.position;
                player.transform.DOMove(movePos, moveDuration);
                //player.Move(MapManager.Instance.fieldRectList[moveIndex].transform.position, 0.25f);

            });
            sequence.AppendInterval(moveDuration);

            // �����ī�� ȿ�� �ߵ�
            //sequence.AppendCallback(() =>
            //{
            //    if (MapManager.Instance.fieldList[moveIndex].isSet == true)
            //    {
            //        MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
            //    }
            //});
            //sequence.AppendInterval(moveDuration);

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
            foreach (BuildCard buildCard in MapManager.Instance.fieldList[moveIndex + i].accessBuildList)
            {
                if (buildCard.isAccesePlayer == true)
                {
                    Debug.Log("accese player");

                    sequence.AppendCallback(() =>
                    {
                        buildCard.AccesePlayer(player);
                    });
                    sequence.AppendInterval(moveDuration * 2);
                }
            }

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

        nextState = GameState.Equip;



        if(isTutoEnd == false && DataManager.Instance.stageNumValue.RuntimeValue == 0)
        {
            isTutoEnd = true;
        }

        // ������ �� ��
        if (loopCountValue.RuntimeValue == bossRound - 1)
        {
            SoundManager.Instance.PlaySFX(SFXType.RandomMonster);
            SoundManager.Instance.PlayBGM(BGMType.Boss);
            enemyController.BossRound();

            isBossEnd = true;
        }
        // �Ϲ� ���϶�
        else if (loopCountValue.RuntimeValue < bossRound - 1)
        {
            SoundManager.Instance.PlaySFX(SFXType.RandomMonster);
            enemyController.CreateRandomMob();
            enemyController.RandomEnemyBuild();

        }
        else
        {
            
        }

        StartCoroutine(Delay(() =>
        {
            if (isFirst == true)
            {
                isFirst = false;
                ShowTuTorialEvent?.Invoke(2);
            }

            shopController.Show();
            selectRewardManager.Show();
            blurController.SetActive(true);

            canNext = true;
        }, 1.5f));
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
        SoundManager.Instance.PlaySFX(SFXType.BasicCardSet);
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
            handleController.cardSorting.AlignCards();

            return false;
        }

        DropArea handleArea = dragbleCard.originDropArea;
        handleArea.TriggerOnLift(dragbleCard);

        DropArea fieldArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        fieldArea.TriggerOnDrop(dragbleCard);

        dragbleCard.transform.rotation = Quaternion.identity;

        handleController.cardSorting.AlignCards();

        HandleCardTooltip.Instance.Hide();

        return true;
    }

    public void LiftField(DragbleCard dragbleCard)
    {
        DropArea fieldArea = dragbleCard.droppedArea;

        if(fieldArea.field.fieldState == FieldState.able)
        {
            fieldArea.TriggerOnLift(dragbleCard);

            DropArea handleArea = dragbleCard.originDropArea;
            handleArea.TriggerOnDrop(dragbleCard);

            dragbleCard.transform.rotation = Quaternion.identity;

            handleController.cardSorting.AlignCards();
        }
    }

    public void StopGame()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            stopImage.sprite = stopSprite;
        }
        else
        {
            Time.timeScale = 1;
            stopImage.sprite = playSprite;
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

        Debug.Log("�÷��̾� ����");
        dontTouchController.Hide();
        playerDieEvent.Occurred();
        DropArea.dropAreas.Clear();
        DontRaycastTarget.dontRaycastTargetList.Clear();
    }

    private void ApplyStateText()
    {
        switch (nextState)
        {
            case GameState.TurnStart:
                nextStateText.text = "���� ����";
                break;
            case GameState.TurnEnd:
                nextStateText.text = "���� ��";
                break;
            case GameState.Move:
                nextStateText.text = "�̵�";
                break;
            case GameState.Modify:
                nextStateText.text = "����";
                break;
            case GameState.Equip:
                nextStateText.text = "�ǹ� ��ġ";
                break;
            case GameState.GameStart:
                nextStateText.text = "���� ����";
                break;
            default:
                break;
        }


        switch (curState)
        {
            case GameState.TurnStart:
                curStateText.text = "���� : ���� ����";
                break;
            case GameState.TurnEnd:
                curStateText.text = "���� : ����";
                break;
            case GameState.Move:
                curStateText.text = "���� : ����";
                break;
            case GameState.Modify:
                curStateText.text = "���� : ����";
                break;
            case GameState.Equip:
                curStateText.text = "���� : �ǹ� ��ġ";
                break;
            case GameState.GameStart:
                curStateText.text = "���� : ���� ����";
                break;
            default:
                break;
        }
    }
} 