using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "ScriptableObject/Stage/StageData")]
public class StageDataSO : ScriptableObject
{
    public int id;
    public string mapStr;
    public List<CardData> deck;
    public int moveCount;       // 한번에 이동할 양
    public bool isDeckShuffle;  // 덱을 섞을지, 튜토리얼에서는 안섞게 하고싶어서
    public int randomMobCount;  // 초기 소환 숫자, 겜메니저에서 점차 늘어나는것
    public int mobIncreaseAmount;  // 몹 늘어나는 양
}
