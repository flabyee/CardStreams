using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataListSO", menuName = "ScriptableObject/Stage/StageDataList")]
public class StageDataListSO : ScriptableObject
{
    public List<StageDataSO> stageDataList;
}
