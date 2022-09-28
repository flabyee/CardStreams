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
    [SerializeField] float moveDuration;    // Move에 걸리는 시간
    [SerializeField] float fieldResetDelay; // 정산이후 대기 시간
    public bool canStartTurn;   // deck을 다 만든 후에 게임을 진행하기 위해서
    public int moveIndex = 0;  // 현재 플레이어가 맵에 위치한 곳
    public GameState curState; // 현재 게임의 상태
    public GameState nextState; // 다음 상태
    public bool canNextLoop;

    // 임시
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

    // 임시 나중에 다른 곳으로 이동
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
    public Action<GameState, GameState> ChangeStateEvent; // 현재 state | 다음 state

    public int mineLevel = 1;

    // 보스 패턴을 위한 Action, CardType을 int로 전환해서 배열에 접근한다
    public Action<CardPower, Vector3>[] ResetFieldEvent;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameManager가 중복되었습니다");
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

        // isFirst 반대로 되어있는 듯?
        isFirst = true;


        LoadStageData();

        fieldController = new FieldController(maxMoveCount);
        fieldController.SetAllFieldYet();

        // 마을 뭐 없으면
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

    // To Do : 나중에 수정
    public void SetPlayerPos() // afterMapCreate 0.03초 늦게부르면 버그안생기고 바로부르면 위치버그생김 뭐지??????????
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
                        UITooltip.Instance.Show("뭘까용~", 1f);
                        return;
                    }

                    // 꽉 차있는지
                    if (fieldController.IsNextFieldFull(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("앞에 4칸을 전부 채운후 다시 시도하세요!!", 1f);
                        return;
                    }

                    // 플레이어 카드가 2장 이하인지
                    if (fieldController.IsNextFieldPlayerCardTwo(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("플레이어 카드를 2장 이하로 배치해주세요", 1f);
                        return;
                    }
                }
                break;

            case GameState.TurnEnd:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("정산중입니다", 1f);
                        return;
                    }
                }
                break;

            case GameState.Move:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("이동중입니다", 1f);
                        return;
                    }

                    // 꽉 차있는지
                    if (fieldController.IsNextFieldFull(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("앞에 4칸을 전부 채운후 다시 시도하세요!!", 1f);
                        return;
                    }

                    // 플레이어 카드가 2장 이하인지
                    if (fieldController.IsNextFieldPlayerCardTwo(moveIndex) == false)
                    {
                        UITooltip.Instance.Show("플레이어 카드를 2장 이하로 배치해주세요", 1f);
                        return;
                    }
                }
                break;

            case GameState.Modify:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("무작위 적이 생성중입니다", 1f);
                        return;
                    }
                }
                break;

            case GameState.Equip:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("뭘까용~", 1f);
                        return;
                    }
                }
                break;

            case GameState.GameStart:
                {
                    if (canNextLoop == false)
                    {
                        UITooltip.Instance.Show("먼저 시작 보상을 선택해주세요", 1f);
                        return;
                    }
                }
                break;
        }

        NextAction();
    }

    public void TurnStart()
    {
        // fieldController.SetAllFieldYet(); // 안해주면 카드가 안들어감 근데 여기다가 이거쓰면 손에보라색몬스터 버그남 ??그럼어떻게해야하지

        // 턴 증가
        loopCountValue.RuntimeValue++;
        loopChangeEvent.Occurred();

        //if(loopCountValue.RuntimeValue == 6)
        //{
        //    // 플레이해주셔서 감사합니다
        //}

        // 앞에 n칸 활성화
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
            // 정산
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

    // 보스전이 아닐 때 필드 초기화하면서 정산하는 코루틴
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

        GoldAnimManager.Instance.GetAllCoin(0f, true);

        yield return new WaitForSeconds(1f);

        missionController.CheckCompleteMission();

        yield return new WaitForSeconds(2f);

        fieldController.SetAllFieldYet();

        canNextLoop = true;
    }

    // 보스전 때 필드 초기화하는 코루틴
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
                    // 필드 리셋
                    Field field = MapManager.Instance.fieldList[i];
                    field.FieldReset();

                    //ResetFieldEvent[(int)cardPower.cardType]?.Invoke(cardPower, field.transform.position);

                    //yield return new WaitForSeconds(fieldResetDelay);
                }
                else
                {
                    // 필드 리셋
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

        // 카드에 건물 효과 적용
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

        // 이전 4개의 필드 상호작용 불가능 하게
        fieldController.SetBeforeFieldNot(moveIndex);
        fieldController.SetMonsterGoldP(moveIndex);

        // 보스라 중이라면 보스와 전투후 뒤로 밀기
        if (IsBossRound())
        {
            Boss bossObj = enemyController.Boss;
            Vector3 playerToBossDir = bossObj.transform.position - player.transform.position;
            Vector3 middlePos = (bossObj.transform.position + player.transform.position) / 2;
            // 전투 애니메이션?
            Sequence sequence = DOTween.Sequence();

            // 뒤로 이동
            sequence.AppendCallback(() =>
            {
                player.transform.DOMove(player.transform.position - playerToBossDir, 0.5f);
                bossObj.MovePos(bossObj.transform.position + playerToBossDir, 0.5f);
            });
            sequence.AppendInterval(0.5f);

            // 격돌
            sequence.AppendCallback(() =>
            {
                player.transform.DOMove(middlePos - (playerToBossDir / 2), 0.25f);
                bossObj.MovePos(middlePos + (playerToBossDir / 2), 0.25f);

                Effects.Instance.TriggerNuclear(middlePos);
            });
            sequence.AppendInterval(0.25f);

            // 원위치
            sequence.AppendCallback(() =>
            {
                player.transform.DOMove(MapManager.Instance.fieldList[moveIndex - 1].transform.position, 0.5f);
                enemyController.Boss.MovePos(MapManager.Instance.fieldList[moveIndex].transform.position, 0.5f);
            });
            sequence.AppendInterval(0.5f);

            yield return new WaitForSeconds(1.5f);


            // 보스와 전투, 플레이어가 죽으면 true
            if (player.OnBoss(enemyController.Boss.Attack, out int sword)) // 플레이어 칼 수치를 out에 담는다
            {
                // 사망 처리
                ResourceManager.Instance.SendSaveFile();
                SaveFile.SaveGame();
                SettingClear();
            }
            else
            {
                // 보스 체력 까기
                enemyController.AttackBoss(sword);

                // 보스 뒤로 미루고 해당 칸 카드 비우기 or 몬스터 0으로 설정하기? 아무튼 배치가 안되야함 
                // 아 아니구나 그냥 뒤로 미루기만 하면됨, 나중에는 그 자리에 랜덤몹이면 힐 되게?
                enemyController.MoveBoss(moveIndex);
            }
        }

        // 마지막이 아니라면 혹은 다음 칸이 있다면
        if (moveIndex != MapManager.Instance.fieldCount)
        {
            // 다음 필드 상호작용 가능하게
            fieldController.SetNextFieldAble(moveIndex);
            // draw
            handleController.DrawCardWhenBeforeMove();

            BuildManager.Instance.NextBuildEffect();
        }

        MoveEndEvent.Occurred();

        dontTouchController.Hide();

        // 다음 차례로
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

        // 필드에 건물 효과 적용
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

        // 4칸 이동
        for (int i = 0; i < maxMoveCount; i++)
        {
            SoundManager.Instance.PlaySFX(SFXType.Moving, 1.5f);

            Field field = MapManager.Instance.fieldList[moveIndex];

            // player 위치 이동
            sequence.AppendCallback(() =>
            {
                Vector3 movePos = field.transform.position;
                player.transform.DOMove(movePos, moveDuration);
                //player.Move(MapManager.Instance.fieldRectList[moveIndex].transform.position, 0.25f);

            });
            sequence.AppendInterval(moveDuration);

            // 스페셜카드 효과 발동
            //sequence.AppendCallback(() =>
            //{
            //    if (MapManager.Instance.fieldList[moveIndex].isSet == true)
            //    {
            //        MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
            //    }
            //});
            //sequence.AppendInterval(moveDuration);

            // 플레이어한테 필드 효과 적용 
            sequence.AppendCallback(() =>
            {
                if (field.isSet == true)
                {
                    player.OnFeild(field);
                }
            });
            sequence.AppendInterval(moveDuration);


            // 플레이어한테 건물효과 적용
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


                // 플레이어 죽었으면 끝
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

        // 다음 루프가 보스가 나오는 루프일때
        if (IsBossEnter())
        {
            SoundManager.Instance.PlaySFX(SFXType.RandomMonster);
            SoundManager.Instance.PlayBGM(BGMType.Boss);
            enemyController.BossRound();
        }
        else
        {
            // 일반 라일때
            SoundManager.Instance.PlaySFX(SFXType.RandomMonster);
            enemyController.CreateEnemyBuild();
            enemyController.CreateRandomMob();
        }

        // 보스가 나온 이후에 못잡고 한바퀴를 돌면 패널티 부여
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
                //if (IsBossRound()) // 보스라운드 깨고 다음으로 넘어갔으면 보상창오픈
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
        bool isPause = Time.timeScale == 0; // 게임 멈췄으면 true

        Time.timeScale = isPause ? 1 : 0; // pause 상태면 재생, not pause면 멈춤
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
        // 현재 보스를 잡는중 이라면
        return loopCountValue.RuntimeValue >= bossRound;
    }

    private bool IsBossEnter()
    {
        // 다음 라운드가 보스라면
        return loopCountValue.RuntimeValue + 1 == bossRound;
    }
}