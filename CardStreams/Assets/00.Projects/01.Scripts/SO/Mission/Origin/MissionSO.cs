using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Mission")]
public abstract class MissionSO : ScriptableObject
{
    public Sprite missionSprite;
    public string missionName;
    [TextArea] 
    public string infoStr;      // 간단하게 왼쪽 위에 표시하기 위한 미션 내용
    [TextArea] 
    public string timInfoStr;   // 상세한 미션 내용
    public MissionGrade grade;

    protected Slider progressSlider;
    protected TextMeshProUGUI progressText;

    public void SetUI(Slider progressSlider, TextMeshProUGUI progressText)
    {
        this.progressSlider = progressSlider;
        this.progressText = progressText;
    }
    public void UnSetUI()
    {
        this.progressSlider = null;
        this.progressText = null;
    }

    // 여기서 감시할 대상의 액션에 자기 추가
    // 매개변수 모두 다르기 때문에 알아서 구현
    // 예시
    //GameManager.Instance.player.OnBasicCardEvent += ObserverUseBasicCard;
    public abstract void GetMission();


    // 추가하려는 감시 대상에 맞는 함수 만들기
    // 예시
    private void ObserverUseBasicCard(BasicCard basicCard)
    {
        // 이런식으로 함수하나 만들고 쓰면 됨
    }

    // UI 모두 다르려나? 때문에 알아서 구현
    public abstract void ApplyUI();

    public abstract bool IsComplete();

    // GetMisiion에서 추가한 이벤트는 Reset에서 제거
    // 예시
    //GameManager.Instance.player.OnBasicCardEvent -= ObserverUseBasicCard;
    public abstract void Reset();
}
