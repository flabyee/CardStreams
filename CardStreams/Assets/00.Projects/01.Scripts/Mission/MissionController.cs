using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    // �̼� ���� ����
    // �̼� UI ����, ����, ��ġ, ���� ��� (���� ������Ʈ�� �̼ǿ��� ��)
    // ���� ���� Ŭ���� �˻��ϴ� �̼� �˻����ֱ�

    public List<Mission> missionList = new List<Mission>();

    public MissionListSO missionListSO;
    public MissionRewardListSO rewardListSO;

    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue hpValue;

    [Header("Event")]
    public EventSO goldChangeEvent;
    public EventSO playerValueChanged;

    // ���� ������ �� ���� �̼� 3�� ȹ��
    public void GetRandomMission()
    {
        Util.RandomList(ref missionListSO.missionList);
        Util.RandomList(ref rewardListSO.rewardList);

        for (int i = 0; i < 3; i++)
        {
            missionList[i].Init(missionListSO.missionList[i]);
            missionList[i].SetMissionReward(rewardListSO.rewardList[i]);
            missionList[i].GetMission();
        }
    }

    // ���� ���� �� �̼ǵ� Ŭ���� ���� Ȯ���ϰ� ���� ȹ��
    public void IsCompleteMission()
    {
        StartCoroutine(CompleteMissionCor());
    }

    public void ResetMissions()
    {
        for (int i = 0; i < 3; i++)
        {
            missionList[i].ResetMission();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetRandomMission();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            IsCompleteMission();
        }
    }

    private IEnumerator CompleteMissionCor()
    {
        // Ŭ���� Ȯ�� �� Ŭ���� ����Ʈ
        bool[] isClearArr = new bool[3] { false, false, false };
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if (missionList[i].IsComplete())
                isClearArr[i] = true;
        }

        yield return new WaitForSeconds(0.5f);

        // ���ÿ� ��� ���� ����
        for (int i = 0; i < 3; i++)
        {
            if(isClearArr[i])
            {
                MissionRewardSO missionReward = missionList[i].GetMissionReward();
                RewardInterprinter(missionList[i], missionReward);
            }
        }
    }

    private void RewardInterprinter(Mission mission, MissionRewardSO missionReward)
    {
        switch (missionReward.rewardType)
        {
            case RewardType.Gold:
                // Bezier�� ��UI�� ������ �����ϸ� ������
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, ConstManager.Instance.goldSprite, TargetType.GoldUI, () =>
                {
                    goldValue.RuntimeValue += missionReward.value;
                    goldChangeEvent.Occurred();
                });
                break;
            case RewardType.Hp:
                // Bezier�� ȸ������ ������ �����ϸ� ȸ��
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, ConstManager.Instance.heartSprite, TargetType.HPUI, () =>
                {
                    hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + missionReward.value, 0, hpValue.RuntimeMaxValue);
                    playerValueChanged.Occurred();
                });
                break;
            case RewardType.MaxHp:
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, ConstManager.Instance.heartSprite, TargetType.HPUI, () =>
                {
                    hpValue.RuntimeMaxValue += missionReward.value;
                    playerValueChanged.Occurred();
                });
                break;
            case RewardType.BuildCard:
                CardGrade grade = (CardGrade)Mathf.Clamp(missionReward.value, 0, 5);
                break;
            case RewardType.SpecialCard:
                break;
            default:
                break;
        }
    }
}
