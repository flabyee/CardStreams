using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    // 미션 랜덤 생성
    // 미션 UI 관리, 생성, 배치, 삭제 등등 (내용 업데이트는 미션에서 함)
    // 루프 이후 클리어 검사하는 미션 검사해주기

    public List<Mission> missionList = new List<Mission>();

    public MissionListSO missionListSO;
    public MissionRewardListSO rewardListSO;

    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue hpValue;

    [Header("Event")]
    public EventSO goldChangeEvent;
    public EventSO playerValueChanged;

    // 루프 시작할 때 랜덤 미션 3개 획득
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

    // 루프 끝날 때 미션들 클리어 여부 확인하고 보상 획득
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
        // 클리어 확인 및 클리어 이펙트
        bool[] isClearArr = new bool[3] { false, false, false };
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if (missionList[i].IsComplete())
                isClearArr[i] = true;
        }

        yield return new WaitForSeconds(0.5f);

        // 동시에 모든 보상 지급
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
                // Bezier로 돈UI로 날리기 도착하면 돈증가
                EffectManager.Instance.GetBezierCardEffect(mission.transform.position, ConstManager.Instance.goldSprite, TargetType.GoldUI, () =>
                {
                    goldValue.RuntimeValue += missionReward.value;
                    goldChangeEvent.Occurred();
                });
                break;
            case RewardType.Hp:
                // Bezier로 회복으로 날리기 도착하면 회복
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
