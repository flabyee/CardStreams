using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildListSO", menuName = "ScriptableObject/Build/BuildListSO")]
public class BuildListSO : ScriptableObject
{
    public List<BuildSO> buildList;
}
