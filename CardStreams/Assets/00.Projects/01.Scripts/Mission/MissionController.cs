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
        Debug.Log(missionList.Count);

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
        for (int i = 0; i < 3; i++)
        {
            if(missionList[i].IsComplete())
            {
                MissionRewardSO missionReward = missionList[i].GetMissionReward();

                switch (missionReward.rewardType)
                {
                    case RewardType.Gold:
                        // Bezier�� ��UI�� ������ �����ϸ� ������
                        EffectManager.Instance.GetBezierCardEffect(missionList[i].transform.position, ConstManager.Instance.goldSprite, TargetType.GoldUI, () =>
                        {
                            goldValue.RuntimeValue += missionReward.value;
                            goldChangeEvent.Occurred();
                        });
                        break;
                    case RewardType.Hp:
                        // Bezier�� ȸ������ ������ �����ϸ� ȸ��
                        EffectManager.Instance.GetBezierCardEffect(missionList[i].transform.position, ConstManager.Instance.heartSprite, TargetType.HPUI, () =>
                        {
                            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + missionReward.value, 0, hpValue.RuntimeMaxValue);
                            playerValueChanged.Occurred();
                        });
                        break;
                    case RewardType.MaxHp:
                        EffectManager.Instance.GetBezierCardEffect(missionList[i].transform.position, ConstManager.Instance.heartSprite, TargetType.HPUI, () =>
                        {
                            hpValue.RuntimeMaxValue += missionReward.value;
                            playerValueChanged.Occurred();
                        });
                        break;
                    case RewardType.BuildCard:
                        break;
                    case RewardType.SpecialCard:
                        break;
                    default:
                        break;
                }
            }
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
}