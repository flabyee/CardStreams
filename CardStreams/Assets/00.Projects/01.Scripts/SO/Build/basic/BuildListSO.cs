using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildListSO", menuName = "ScriptableObject/Build/BuildListSO")]
public class BuildListSO : ScriptableObject
{
    public List<BuildSO> buildList;

    //public List<BuildSO> commonList;
    //public List<BuildSO> rareList;
    //public List<BuildSO> epicList;
    //public List<BuildSO> uniqueList;
    //public List<BuildSO> legendarayList;
}
