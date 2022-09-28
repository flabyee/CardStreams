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


    [HideInInspector] public int rerollCount;


    [Header("Player")]
    public Player player;

    [Header("System")]
    [SerializeField] float moveDuration;    // Move�� �ɸ��� �ð�
    [SerializeField] float fieldResetDelay; // �������� ��� �ð�
    public bool canStartTurn;   // deck�� �� ���� �Ŀ� ������ �����ϱ� ���ؼ�
    public int moveIndex = 0;  // ���� �÷��̾ �ʿ� ��ġ�� ��
    public GameState curState; // ���� ������ ����
    public GameState nextState; // ���� ����
    public bool canNextLoop;

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
    public DontTouchController dontTouchController;
    public BlurCoverController blurController;

    public MissionController missionController;

    //[Header("UI")]
    //public Text nextStateText;
    //public Text curStateText;

    // �ӽ� ���߿� �ٸ� ������ �̵�
    public GameObject tutoEndPanel;
    public GameObject clearPanel;

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


    [Header("Action")]
    public Action<int> ShowTuTorialEvent;
    public Action<GameState, GameState> ChangeStateEvent; // ���� state | ���� state

    public int mineLevel = 1;

    // ���� ������ ���� Action, CardType�� int�� ��ȯ�ؼ� �迭�� �����Ѵ�
    public Action<CardPower, Vector3>[] ResetFieldEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameManager�� �ߺ��Ǿ����ϴ�");
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        if (DataManager.Instance.stageNumValue.RuntimeValue == 0)
        {
            curState = GameState.GameStart;
            nextState = GameState.TurnStart;
            canNextLoop = true;
        }
        else
        {
            curState = GameState.GameStart;
            nextState = GameState.Modify;
            canNextLoop = false;
        }

        // ApplyStateText();
        ChangeStateEvent?.Invoke(curState, nextState);

        // isFirst �ݴ�� �Ǿ��ִ� ��?
        isFirst = true;


        LoadStageData();

        fieldController = new FieldController(maxMoveCount);
        fieldController.SetAllFieldYet();

        // ���� �� ������
        if (goldValue.RuntimeMaxValue <= 0)
        {
            goldValue.RuntimeValue += 20;
            goldChangeEvent.Occurred();
        }

        loopCountValue.RuntimeValue = 0;
        loopChangeEvent.Occurred();
    }

    public void LoadStageData()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();
        maxMoveCount = stageData.moveCount;
        bossRound = stageData.bossRound;
    }

    // To Do : ���߿� ����
    public void SetPlayerPos() // afterMapCreate 0.03�� �ʰԺθ��� ���׾Ȼ���� �ٷκθ��� ��ġ���׻��� ����??????????
    {
        Vector3 movePos = MapManager.Instance.fieldList[MapManager.Instance.fieldCount - 1].transform.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(player.transform.DOMove(movePos, 0.25f));
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

        // ApplyStateText();
        ChangeStateEvent?.Invoke(curState, nextState);
    }

    public void OnClickAction()
    {
        SoundManager.Instance.PlaySFX(SFXType.NextButton);

        switch (curState)
        {
            case GameState.TurnStart:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("�����~", 1f);
                        return;
                    }

                    // �� ���ִ���
                    if (fieldController.IsNextFieldFull(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("�տ� 4ĭ�� ���� ä���� �ٽ� �õ��ϼ���!!", 1f);
                        return;
                    }

                    // �÷��̾� ī�尡 2�� ��������
                    if (fieldController.IsNextFieldPlayerCardTwo(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("�÷��̾� ī�带 2�� ���Ϸ� ��ġ���ּ���", 1f);
                        return;
                    }
                }
                break;

            case GameState.TurnEnd:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("�������Դϴ�", 1f);
                        return;
                    }
                }
                break;

            case GameState.Move:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("�̵����Դϴ�", 1f);
                        return;
                    }

                    // �� ���ִ���
                    if (fieldController.IsNextFieldFull(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("�տ� 4ĭ�� ���� ä���� �ٽ� �õ��ϼ���!!", 1f);
                        return;
                    }

                    // �÷��̾� ī�尡 2�� ��������
                    if (fieldController.IsNextFieldPlayerCardTwo(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("�÷��̾� ī�带 2�� ���Ϸ� ��ġ���ּ���", 1f);
                        return;
                    }
                }
                break;

            case GameState.Modify:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("������ ���� �������Դϴ�", 1f);
                        return;
                    }
                }
                break;

            case GameState.Equip:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("�����~", 1f);
                        return;
                    }
                }
                break;

            case GameState.GameStart:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("���� ���� ������ �������ּ���", 1f);
                        return;
                    }
                }
                break;
        }

        NextAction();
    }

    public void TurnStart()
    {
        // fieldController.SetAllFieldYet(); // �����ָ� ī�尡 �ȵ� �ٵ� ����ٰ� �̰ž��� �տ���������� ���׳� ??�׷�����ؾ�����

        // �� ����
        loopCountValue.RuntimeValue++;
        loopChangeEvent.Occurred();

        //if(loopCountValue.RuntimeValue == 6)
        //{
        //    // �÷������ּż� �����մϴ�
        //}

        // �տ� nĭ Ȱ��ȭ
        fieldController.SetNextFieldAble(moveIndex);

        handleController.HandleReturnToDeck();
        handleController.DrawCardWhenBeforeMove();
        handleController.notHaveBuildUI.SetActive(false);

        missionController.GetRandomMission();

        BuildManager.Instance.NextBuildEffect();

        TurnStartEvent.Occurred();

        canNextLoop = true;

        if (isFirst == true)
            ShowTuTorialEvent?.Invoke(1);

        MissionObserverManager.instance.ResetTimer();

        nextState = GameState.Move;
    }

    public void TurnEnd()
    {
        canNextLoop = false;
        moveIndex = 0;

        handleController.LoopEnd();
        ResourceManager.Instance.AddResource(ResourceType.crystal, loopCountValue.RuntimeValue * 5);
        // crystalChangeEvent?.Occurred();


        if (IsBossRound() == false)
        {
            // ����
            StartCoroutine(JungSanCor());
            nextState = GameState.Modify;
        }
        else
        {
            StartCoroutine(FieldResetCor());
            TurnEndEvent.Occurred();
            nextState = GameState.TurnStart;
        }

        EffectManager.Instance.DeleteNextBuildEffect();

        if (isTutoEnd == true)
        {
            nextState = GameState.TutoEnd;
        }
    }

    // �������� �ƴ� �� �ʵ� �ʱ�ȭ�ϸ鼭 �����ϴ� �ڷ�ƾ
    public IEnumerator JungSanCor()
    {
        for (int i = 0; i < MapManager.Instance.fieldCount; i++)
        {
            Field nowField = MapManager.Instance.fieldList[i];

            if (nowField.isSet == true)
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

        GoldAnimManager.Instance.GetAllCoin(0f, true);

        yield return new WaitForSeconds(1f);

        missionController.CheckCompleteMission();

        yield return new WaitForSeconds(2f);

        fieldController.SetAllFieldYet();

        canNextLoop = true;
    }

    // ������ �� �ʵ� �ʱ�ȭ�ϴ� �ڷ�ƾ
    public IEnumerator FieldResetCor()
    {
        for (int i = 0; i < MapManager.Instance.fieldCount; i++)
        {
            Field nowField = MapManager.Instance.fieldList[i];

            if (nowField.isSet == true)
            {
                BasicCard cardPower = nowField.cardPower as BasicCard;
                if (cardPower.basicType == BasicType.Monster)
                {
                    // �ʵ� ����
                    Field field = MapManager.Instance.fieldList[i];
                    field.FieldReset();

                    //ResetFieldEvent[(int)cardPower.cardType]?.Invoke(cardPower, field.transform.position);

                    //yield return new WaitForSeconds(fieldResetDelay);
                }
                else
                {
                    // �ʵ� ����
                    nowField.FieldReset();
                }
            }
        }

        yield return new WaitForSeconds(1f);

        fieldController.SetAllFieldYet();

        canNextLoop = true;
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
        StartCoroutine(MoveEndCor());
    }

    public IEnumerator MoveEndCor()
    {
        canNextLoop = true;

        // ���� 4���� �ʵ� ��ȣ�ۿ� �Ұ��� �ϰ�
        fieldController.SetBeforeFieldNot(moveIndex);
        fieldController.SetMonsterGoldP(moveIndex);

        // ������ ���̶�� ������ ������ �ڷ� �б�
        if (IsBossRound())
        {
            Boss bossObj = enemyController.Boss;
            Vector3 playerToBossDir = bossObj.transform.position - player.transform.position;
            Vector3 middlePos = (bossObj.transform.position + player.transform.position) / 2;
            // ���� �ִϸ��̼�?
            Sequence sequence = DOTween.Sequence();

            // �ڷ� �̵�
            sequence.AppendCallback(() =>
            {
                player.transform.DOMove(player.transform.position - playerToBossDir, 0.5f);
                bossObj.MovePos(bossObj.transform.position + playerToBossDir, 0.5f);
            });
            sequence.AppendInterval(0.5f);

            // �ݵ�
            sequence.AppendCallback(() =>
            {
                player.transform.DOMove(middlePos - (playerToBossDir / 2), 0.25f);
                bossObj.MovePos(middlePos + (playerToBossDir / 2), 0.25f);

                Effects.Instance.TriggerNuclear(middlePos);
            });
            sequence.AppendInterval(0.25f);

            // ����ġ
            sequence.AppendCallback(() =>
            {
                player.transform.DOMove(MapManager.Instance.fieldList[moveIndex - 1].transform.position, 0.5f);
                enemyController.Boss.MovePos(MapManager.Instance.fieldList[moveIndex].transform.position, 0.5f);
            });
            sequence.AppendInterval(0.5f);

            yield return new WaitForSeconds(1.5f);


            // ������ ����, �÷��̾ ������ true
            if (player.OnBoss(enemyController.Boss.Attack, out int sword)) // �÷��̾� Į ��ġ�� out�� ��´�
            {
                // ��� ó��
                ResourceManager.Instance.SendSaveFile();
                SaveFile.SaveGame();
                SettingClear();
            }
            else
            {
                // ���� ü�� ���
                enemyController.AttackBoss(sword);

                // ���� �ڷ� �̷�� �ش� ĭ ī�� ���� or ���� 0���� �����ϱ�? �ƹ�ư ��ġ�� �ȵǾ��� 
                // �� �ƴϱ��� �׳� �ڷ� �̷�⸸ �ϸ��, ���߿��� �� �ڸ��� �������̸� �� �ǰ�?
                enemyController.MoveBoss(moveIndex);
            }
        }

        // �������� �ƴ϶�� Ȥ�� ���� ĭ�� �ִٸ�
        if (moveIndex != MapManager.Instance.fieldCount)
        {
            // ���� �ʵ� ��ȣ�ۿ� �����ϰ�
            fieldController.SetNextFieldAble(moveIndex);
            // draw
            handleController.DrawCardWhenBeforeMove();

            BuildManager.Instance.NextBuildEffect();
        }

        MoveEndEvent.Occurred();

        dontTouchController.Hide();

        // ���� ���ʷ�
        if (moveIndex == MapManager.Instance.fieldCount)
        {
            nextState = GameState.TurnEnd;

            NextAction();
        }

        yield return null;
    }

    public void Move()
    {
        Sequence sequence = DOTween.Sequence();

        canNextLoop = false;

        // Move Start
        sequence.AppendCallback(() => MoveStart());

        // �ʵ忡 �ǹ� ȿ�� ����
        for (int i = 0; i < maxMoveCount; i++)
        {
            Field field = MapManager.Instance.fieldList[moveIndex + i];

            foreach (BuildCard buildCard in field.accessBuildList)
            {
                if (buildCard.isAccessCard == true)
                {
                    sequence.AppendCallback(() =>
                    {
                        buildCard.AcceseCard(field);
                    });
                    sequence.AppendInterval(moveDuration * 2);
                }
            }
        }

        sequence.AppendInterval(moveDuration * 3);

        // 4ĭ �̵�
        for (int i = 0; i < maxMoveCount; i++)
        {
            SoundManager.Instance.PlaySFX(SFXType.Moving, 1.5f);

            Field field = MapManager.Instance.fieldList[moveIndex];

            // player ��ġ �̵�
            sequence.AppendCallback(() =>
            {
                Vector3 movePos = field.transform.position;
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
                if (field.isSet == true)
                {
                    player.OnFeild(field);
                }
            });
            sequence.AppendInterval(moveDuration);


            // �÷��̾����� �ǹ�ȿ�� ����
            foreach (BuildCard buildCard in field.accessBuildList)
            {
                if (buildCard.isAccessPlayer == true)
                {
                    sequence.AppendCallback(() =>
                    {
                        buildCard.AccesePlayer(player);
                    });
                    sequence.AppendInterval(moveDuration * 2);
                }
            }

            moveIndex++;

            sequence.AppendCallback(() => {


                // �÷��̾� �׾����� ��
                if (player.isAlive == false)
                {
                    ResourceManager.Instance.SendSaveFile();
                    SaveFile.SaveGame();
                    SettingClear();

                    sequence.Kill();
                    return;
                }
            });
            sequence.AppendInterval(moveDuration);
        }

        // Move End
        sequence.AppendCallback(() => MoveEnd());
    }

    private void OnModify()
    {
        missionController.ResetMissions();

        canNextLoop = false;

        nextState = GameState.Equip;

        if (isTutoEnd == false && DataManager.Instance.stageNumValue.RuntimeValue == 0)
        {
            isTutoEnd = true;
        }

        // ���� ������ ������ ������ �����϶�
        if (IsBossEnter())
        {
            SoundManager.Instance.PlaySFX(SFXType.RandomMonster);
            SoundManager.Instance.PlayBGM(BGMType.Boss);
            enemyController.BossRound();
        }
        else
        {
            // �Ϲ� ���϶�
            SoundManager.Instance.PlaySFX(SFXType.RandomMonster);
            enemyController.CreateEnemyBuild();
            enemyController.CreateRandomMob();
        }

        // ������ ���� ���Ŀ� ����� �ѹ����� ���� �г�Ƽ �ο�
        if (IsBossRound())
        {
            canNextLoop = true;
        }
        else
        {
            StartCoroutine(Util.DelayCoroutine(1.5f, () =>
            {
                if (isFirst == true)
                {
                    isFirst = false;
                    ShowTuTorialEvent?.Invoke(2);
                }

                shopController.Show();
                //if (IsBossRound()) // �������� ���� �������� �Ѿ���� ����â����
                //{
                //    selectRewardManager.Show();
                //}
                blurController.SetActive(true);
                canNextLoop = true;
            }));
        }
    }

    private void OnEquip()
    {
        shopController.Hide();
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

        if (fieldArea.field.fieldState == FieldState.able)
        {
            fieldArea.TriggerOnLift(dragbleCard);

            DropArea handleArea = dragbleCard.originDropArea;
            handleArea.TriggerOnDrop(dragbleCard);

            dragbleCard.transform.rotation = Quaternion.identity;

            handleController.cardSorting.AlignCards();
        }
    }

    public void PauseGame() // for KeyShortcutManager
    {
        bool isPause = Time.timeScale == 0; // ���� �������� true

        Time.timeScale = isPause ? 1 : 0; // pause ���¸� ���, not pause�� ����
    }

    public void SetMoveDuration(float speed)
    {
        moveDuration = speed;
    }

    public void SettingClear()
    {
        goldValue.RuntimeValue = 0;
        player.hpValue.RuntimeValue = 0;
        player.hpValue.RuntimeMaxValue = 0;

        dontTouchController.Hide();
        playerDieEvent.Occurred();
        DropArea.dropAreas.Clear();
        DontRaycastTarget.dontRaycastTargetList.Clear();
    }



    private bool IsBossRound()
    {
        // ���� ������ ����� �̶��
        return loopCountValue.RuntimeValue >= bossRound;
    }

    private bool IsBossEnter()
    {
        // ���� ���尡 �������
        return loopCountValue.RuntimeValue + 1 == bossRound;
    }
}