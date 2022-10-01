using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveList", menuName = "ScriptableObject/Buff/PassiveList")]
public class PassiveListSO : ScriptableObject
{
    public List<PassiveSO> dontDestroyPassiveList;
}