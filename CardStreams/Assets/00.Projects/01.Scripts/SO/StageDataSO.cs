using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "ScriptableObject/Stage/StageData")]
public class StageDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public int id;
    public string mapStr;
    public int moveCount;       // 한번에 이동할 양

    [Header("덱 관련 정보")]
    public bool isDeckShuffle;  // 덱을 섞을지, 튜토리얼에서는 안섞게 하고싶어서
    public int firstDeckValueAmount;  // 초기 가중치 합
    public int deckValueIncreaseAmount; // 가중치 줄어드는 양
    public float deckValueIncreaseMultipication;  // 가중치 줄어드는 양의 배율


    [Header("랜덤 몹 관련 정보")]
    public int firstMobSpawnAmount;  // 초기 소환 숫자, 겜메니저에서 점차 늘어나는것
    public int mobIncreaseAmount;  // 몹 늘어나는 양
    public int firstMobAttackAmount;            // 몹의 공격력
    public int mobAttackIncreaseAmount;     // 몹 공격력 증가량

    [Header("보스 관련 정보")]
    public int bossRound;   // 보스가 나오는 루프수
    public int bossValue;
    public int downValue;   // 보스가 나오는 라운드에 약화되는 덱의 수치
}
