using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageNPCHouse", menuName = "ScriptableObject/Build/Village/NPCHouse")]
public class VillageNPCHouseSO : VillageBuildSO
{
    public NpcSO npcSO;
    public GameObject npcPrefab;
}
