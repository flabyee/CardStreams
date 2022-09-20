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
    // UI도 필수적인 부분만 여기서 선언하고 나머지는 알아서
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionInfoText;
    public Image gradeImage;
    public Slider progressSlider;
    public TextMeshProUGUI progressText;

    public Image backgroundImage;

    // mission reward UI 필요함
    private MissionRewardSO missionRewardSO;


    public Action GetMissionEvent;
    public Action ApplyUIEvent;
    public Func<bool> IsCompleteEvent;

    private MissionSO missionSO;

    public void Init(MissionSO missionSO)
    {
        // Action 연동
        GetMissionEvent += missionSO.GetMission;
        ApplyUIEvent += missionSO.ApplyUI;
        IsCompleteEvent += missionSO.IsComplete;

        // 가변 UI 전달
        missionSO.SetUI(progressSlider, progressText);

        // 고정 UI 설정
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

    // 성공여부는 한바퀴돈 이후에 체크
    public bool IsComplete()
    {
        if (IsCompleteEvent == null)
        {
            Debug.LogError("IsCompleteEvent에 조건이 할당되지 않음");
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
        // Action 연동 해제
        GetMissionEvent = null;
        ApplyUIEvent = null;
        IsCompleteEvent = null;

        // 가변 UI 할당 해제 및 초기화
        missionSO.UnSetUI();
        progressText.color = Color.white;
        progressText.text = string.Empty;
        progressSlider.value = 0;

        // 고정 UI 초기화
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
