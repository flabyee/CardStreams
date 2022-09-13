using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageBuffList", menuName = "ScriptableObject/Buff/VillageBuffList")]
public class VillageBuffListSO : ScriptableObject
{
    public List<BuffSO> buffList;
}
