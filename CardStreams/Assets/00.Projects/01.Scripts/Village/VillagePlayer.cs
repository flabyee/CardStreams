using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VillageState
{
    Idle, // 가만히있음(시작 or 상점UI 보는중)
    Move, // 이동중
}

public class VillagePlayer : Player
{
    [Header("이동")]
    private VillageState curState;
    private int mapIndex = 0; // 루프되는 길의 몇번째 칸에 있는지
    private const float movingTime = 0.5f; // 다음 타일까지 걸리는 초기 시간
    private float remainTime = 0.5f; // 다음 타일 가기까지 남은시간

    [Header("필드")]
    [HideInInspector] public VillageField curField;
    private VillageField destField;

    [Header("패시브")]
    public PassiveSystem passiveSystem;

    protected override void Awake()
    {
        // IntValue Init
        hpValue.RuntimeMaxValue = hpValue.InitialMaxValue;
        hpValue.RuntimeValue = hpValue.InitialValue;

        base.Awake();
    }

    private void Start()
    {
        buffListSO.buffList.Clear(); // 마을 시작할때 마을버프 싹다 리셋
        buffListSO = null; // 참조 풀어서 player에 있는 ondestroy 작동안하게
        StartCoroutine(Util.DelayCoroutine(0.5f, () => Init()));
    }

    private void Update()
    {
        if(curState == VillageState.Move) // 이동중이면 시간 계산
        {
            remainTime -= Time.deltaTime;
        }
    }

    // 외부에서 호출할수도 있는 함수들

    public void Init() // 플레이어 위치 초기화시킬때 호출하기
    {
        mapIndex = 0;
        remainTime = movingTime;
        curField = VillageMapManager.Instance.fieldList[0];
        destField = VillageMapManager.Instance.fieldList[1];
        transform.position = curField.transform.position;
        MoveStart();
    }

    public void MoveStart() // 플레이어 이동시킬때 호출하기
    {
        SetState(VillageState.Move);
        MoveNextTile();
    }

    public void Stop() // 플레이어 멈출때 호출하기
    {
        transform.DOKill();
        SetState(VillageState.Idle);
    }

    // 멤버함수들

    private void MoveNextTile()
    {
        // SoundManager.Instance.PlaySFX(SFXType.Moving);
        transform.DOMove(destField.transform.position, remainTime).OnComplete(() =>
        {
            ArriveTile();
        });
    }

    private void SetState(VillageState state)
    {
        curState = state;
    }

    private void ArriveTile() // 목표 타일 도착
    {
        // 타일 도착시 실행
        foreach (BuildCard buildCard in destField.accessBuildList)
        {
            var buildSO = buildCard.buildSO as VillageBuildSO;
            if (buildSO != null)
            {
                buildSO.AccessPlayer(this);
            }
        }

        foreach (Npc npc in destField.accessNpcList)
        {
            npc.AccessPlayer(this);
        }

        // 다음 목표타일 설정
        mapIndex++;
        remainTime = movingTime;
        curField = destField;
        if (mapIndex < VillageMapManager.Instance.fieldList.Count)
        {
            destField = VillageMapManager.Instance.fieldList[mapIndex];

                // 다시 이동
                if (curState != VillageState.Move) return;

            MoveNextTile();
        }
        else
        {
            // 마지막 타일 도착했으니 정리하고 던전으로
            Debug.Log("마지막 타일에 도착했습니다");
            passiveSystem.AddToPlayerBuffList();
            LoadingSceneManager.LoadScene("SampleScene");
        }
    }
}
