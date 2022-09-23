using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/MissionList")]
public class MissionListSO : ScriptableObject
{
    public List<MissionSO> easyList;
    public List<MissionSO> normalList;
    public List<MissionSO> hardList;
}
