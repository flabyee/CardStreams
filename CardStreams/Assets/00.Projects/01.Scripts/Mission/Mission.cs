using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum MissionGrade
{
    Easy,
    Normal,
    Hard,
}

public class Mission : MonoBehaviour
{
    // UI�� �ʼ����� �κи� ���⼭ �����ϰ� �������� �˾Ƽ�
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionInfoText;
    public Image gradeImage;
    public Slider progressSlider;
    public TextMeshProUGUI progressText;

    public Image backgroundImage;

    // mission reward UI �ʿ���
    private MissionRewardSO missionRewardSO;


    public Action GetMissionEvent;
    public Action ApplyUIEvent;
    public Func<bool> IsCompleteEvent;

    private MissionSO missionSO;

    public void Init(MissionSO missionSO)
    {
        // Action ����
        GetMissionEvent += missionSO.GetMission;
        ApplyUIEvent += missionSO.ApplyUI;
        IsCompleteEvent += missionSO.IsComplete;

        // ���� UI ����
        missionSO.SetUI(progressSlider, progressText);

        // ���� UI ����
        gradeImage.sprite = missionSO.missionSprite;
        missionInfoText.text = missionSO.infoStr;

        this.missionSO = missionSO;
    }

    public void GetMission()
    {
        GetMissionEvent?.Invoke();

        ApplyUI();
    }

    public void ApplyUI()
    {
        ApplyUIEvent?.Invoke();
    }

    // �������δ� �ѹ����� ���Ŀ� üũ
    public bool IsComplete()
    {
        if (IsCompleteEvent == null)
        {
            Debug.LogError("IsCompleteEvent�� ������ �Ҵ���� ����");
            return false;
        }
        
        CompleteEffect(IsCompleteEvent.Invoke());
        return IsCompleteEvent.Invoke();
    }

    public void SetMissionReward(MissionRewardSO missionRewardSO)
    {
        this.missionRewardSO = missionRewardSO;
    }

    public MissionRewardSO GetMissionReward()
    {
        return missionRewardSO;
    }

    public void ResetMission()
    {
        // Action ���� ����
        GetMissionEvent = null;
        ApplyUIEvent = null;
        IsCompleteEvent = null;

        // ���� UI �Ҵ� ���� �� �ʱ�ȭ
        missionSO.UnSetUI();
        progressText.color = Color.white;
        progressText.text = string.Empty;
        progressSlider.value = 0;

        // ���� UI �ʱ�ȭ
        gradeImage.sprite = null;
        missionInfoText.text = string.Empty;
        backgroundImage.color = Color.white;

        missionSO = null;
    }

    public void CompleteEffect(bool isComplete)
    {
        if(isComplete)
        {
            backgroundImage.color = Color.green;
        }
        else
        {
            backgroundImage.color = Color.red;
        }
    }
}
