using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MissionType
{
    Now,
    AfterLoop,
}

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

    public virtual void Complete()
    {
        // 보수
    }

    public virtual void GetMission()
    {
        // 여기서 감시할 대상의 액션에 자기 추가
        // 매개변수 모두 다르기 때문에 알아서 구현

        // UI도 설정
        ApplyUI();
    }

    public virtual void ApplyUI()
    {

    }
}
