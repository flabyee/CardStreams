using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    // 미션 랜덤 생성
    // 미션 UI 관리, 생성, 배치, 삭제 등등 (내용 업데이트는 미션에서 함)
    // 루프 이후 클리어 검사하는 미션 검사해주기

    public static MissionManager instance;
    public GameObject missionPrefab;
    public RectTransform missionParentTrm;

    private List<Mission> missionList;

    // 루프 시작할 때 랜덤 미션 3개 획득
    public void GetRandomMission()
    {

    }

    // 루프 끝날 때 미션들 클리어 여부 확인하고 보상 획득
    public void IsCompleteMission()
    {

    }
}
