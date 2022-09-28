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
    public bool isTestMode = false;

    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue hpValue;

    [Header("Event")]
    public EventSO goldChangeEvent;
    public EventSO playerValueChanged;

    private void Awake()
    {
        if(isTestMode == false)
        {
            missionListSO = Resources.Load<MissionListSO>("MissionList");
            rewardListSO = Resources.Load<MissionRewardListSO>("MissionRewardList");
        }
        else
        {
            missionListSO = Resources.Load<MissionListSO>("MissionList_Test");
            rewardListSO = Resources.Load<MissionRewardListSO>("MissionRewardList_Test");
        }
    }

    private void Update()
    {
        if(isTestMode == true && Input.GetKeyDown(KeyCode.T))
        {
            CheckCompleteMission();
        }
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
                missionList[i + missionIndex].SetMissionReward(GetRewardSO(MissionGrade.Easy));

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
                missionList[i + missionIndex].SetMissionReward(GetRewardSO(MissionGrade.Normal));

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
                missionList[i + missionIndex].SetMissionReward(GetRewardSO(MissionGrade.Hard));

                missionList[i + missionIndex].GetMission();
            }

            missionIndex += missionGradeList[index].hard;
        }

        index = Mathf.Clamp(index + 1, 0, missionGradeList.Count);
    }

    // ���� ���� �� �̼ǵ� Ŭ���� ���� Ȯ���ϰ� ���� ȹ��
    public void CheckCompleteMission()
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
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, null, TargetType.Bag, null);
                GameManager.Instance.handleController.AddBuild(DataManager.Instance.GetRandomBuildSO((CardGrade)missionReward.value).id);
                break;

            case RewardType.SpecialCard:
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, null, TargetType.Bag, null);
                GameManager.Instance.handleController.AddSpecial(DataManager.Instance.GetRandomSpecialSO((CardGrade)missionReward.value).id);
                break;

            case RewardType.Exp:
                GameManager.Instance.player.GetExpBezier(missionReward.value, mission.transform.position);
                break;

            case RewardType.Crystal:
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, null, TargetType.GoldUI, () =>
                {
                    ResourceManager.Instance.AddResource(ResourceType.crystal, missionReward.value);
                });
                break;

            case RewardType.Prestige:
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, null, TargetType.GoldUI, () =>
                {
                    ResourceManager.Instance.AddResource(ResourceType.prestige, missionReward.value);
                });
                break;

            default:
                Debug.LogError("������ ���� �ȵǾ��ִ� ����");
                break;
        }
    }

    private MissionRewardSO GetRewardSO(MissionGrade grade)
    {
        switch (grade)
        {
            case MissionGrade.Easy:
                return rewardListSO.easyList[Random.Range(0, rewardListSO.easyList.Count)];
            case MissionGrade.Normal:
                return rewardListSO.normalList[Random.Range(0, rewardListSO.normalList.Count)];
            case MissionGrade.Hard:
                return rewardListSO.hardList[Random.Range(0, rewardListSO.hardList.Count)];
            default:
                return rewardListSO.easyList[Random.Range(0, rewardListSO.easyList.Count)];
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
