using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    // �̼� ���� ����
    // �̼� UI ����, ����, ��ġ, ���� ��� (���� ������Ʈ�� �̼ǿ��� ��)
    // ���� ���� Ŭ���� �˻��ϴ� �̼� �˻����ֱ�

    public List<Mission> missionList = new List<Mission>();

    private MissionListSO missionListSO;
    private MissionRewardListSO rewardListSO;

    [Header("Debug")]
    public List<MissionGrades> missionGradeList;
    private int index;

    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue hpValue;

    [Header("Event")]
    public EventSO goldChangeEvent;
    public EventSO playerValueChanged;

    private void Awake()
    {
        missionListSO = Resources.Load<MissionListSO>("MissionList");
        rewardListSO = Resources.Load<MissionRewardListSO>("MissionRewardList");
    }

    // ���� ������ �� ���� �̼� 3�� ȹ��
    public void GetRandomMission()
    {
        if (missionGradeList[index].easy + missionGradeList[index].normal + missionGradeList[index].hard != 3)
        {
            Debug.LogError("�̼��� 3���� �Ҵ����־�� �մϴ�");
            return;
        }

        int missionIndex = 0;

        if(missionGradeList[index].easy > 0)
        {
            Util.RandomList(ref missionListSO.easyList);
            Util.RandomList(ref rewardListSO.easyList);

            for (int i = 0; i < missionGradeList[index].easy; i++)
            {
                missionList[i + missionIndex].Init(missionListSO.easyList[i]);
                missionList[i + missionIndex].SetMissionReward(rewardListSO.easyList[i]);

                missionList[i + missionIndex].GetMission();
            }

            missionIndex += missionGradeList[index].easy;
        }

        if (missionGradeList[index].normal > 0)
        {
            Util.RandomList(ref missionListSO.normalList);
            Util.RandomList(ref rewardListSO.normalList);

            for (int i = 0; i < missionGradeList[index].normal; i++)
            {
                missionList[i + missionIndex].Init(missionListSO.normalList[i]);
                missionList[i + missionIndex].SetMissionReward(rewardListSO.normalList[i]);

                missionList[i + missionIndex].GetMission();
            }

            missionIndex += missionGradeList[index].normal;
        }

        if (missionGradeList[index].hard > 0)
        {
            Util.RandomList(ref missionListSO.hardList);
            Util.RandomList(ref rewardListSO.hardList);

            for (int i = 0; i < missionGradeList[index].hard; i++)
            {
                missionList[i + missionIndex].Init(missionListSO.hardList[i]);
                missionList[i + missionIndex].SetMissionReward(rewardListSO.hardList[i]);

                missionList[i + missionIndex].GetMission();
            }

            missionIndex += missionGradeList[index].hard;
        }

        index = Mathf.Clamp(index + 1, 0, missionGradeList.Count);
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
        CardGrade grade = (CardGrade)Mathf.Clamp(missionReward.value, 0, 5);

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
                grade++;
                break;
            case RewardType.SpecialCard:
                break;
            case RewardType.Exp:
                break;
            case RewardType.Crystal:
                break;
            default:
                break;
        }
    }

    [System.Serializable]
    public struct MissionGrades
    {
        public int easy;
        public int normal;
        public int hard;
    }
}
