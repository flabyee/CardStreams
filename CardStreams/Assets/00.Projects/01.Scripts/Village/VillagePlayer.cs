using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VillageState
{
    Idle, // ����������(���� or ����UI ������)
    Move, // �̵���
}

public class VillagePlayer : Player
{
    [Header("�̵�")]
    private VillageState curState;
    private int mapIndex = 0; // �����Ǵ� ���� ���° ĭ�� �ִ���
    private const float movingTime = 0.5f; // ���� Ÿ�ϱ��� �ɸ��� �ʱ� �ð�
    private float remainTime = 0.5f; // ���� Ÿ�� ������� �����ð�

    [Header("�ʵ�")]
    [HideInInspector] public VillageField curField;
    private VillageField destField;

    [Header("�нú�")]
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
        buffListSO.buffList.Clear(); // ���� �����Ҷ� �������� �ϴ� ����
        buffListSO = null; // ���� Ǯ� player�� �ִ� ondestroy �۵����ϰ�
        StartCoroutine(Util.DelayCoroutine(0.5f, () => Init()));
    }

    private void Update()
    {
        if(curState == VillageState.Move) // �̵����̸� �ð� ���
        {
            remainTime -= Time.deltaTime;
        }
    }

    // �ܺο��� ȣ���Ҽ��� �ִ� �Լ���

    public void Init() // �÷��̾� ��ġ �ʱ�ȭ��ų�� ȣ���ϱ�
    {
        mapIndex = 0;
        remainTime = movingTime;
        curField = VillageMapManager.Instance.fieldList[0];
        destField = VillageMapManager.Instance.fieldList[1];
        transform.position = curField.transform.position;
        MoveStart();
    }

    public void MoveStart() // �÷��̾� �̵���ų�� ȣ���ϱ�
    {
        SetState(VillageState.Move);
        MoveNextTile();
    }

    public void Stop() // �÷��̾� ���⶧ ȣ���ϱ�
    {
        transform.DOKill();
        SetState(VillageState.Idle);
    }

    // ����Լ���

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

    private void ArriveTile() // ��ǥ Ÿ�� ����
    {
        // Ÿ�� ������ ����
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

        // ���� ��ǥŸ�� ����
        mapIndex++;
        remainTime = movingTime;
        curField = destField;
        if (mapIndex < VillageMapManager.Instance.fieldList.Count)
        {
            destField = VillageMapManager.Instance.fieldList[mapIndex];

                // �ٽ� �̵�
                if (curState != VillageState.Move) return;

            MoveNextTile();
        }
        else
        {
            // ������ Ÿ�� ���������� �����ϰ� ��������
            Debug.Log("������ Ÿ�Ͽ� �����߽��ϴ�");
            passiveSystem.AddToPlayerBuffList();
            LoadingSceneManager.LoadScene("SampleScene");
        }
    }
}
