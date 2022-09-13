using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcSpawnData", menuName = "ScriptableObject/NPC/NPCData")]
public class NpcSpawnDataSO : ScriptableObject
{
    public VillageNPCHouseSO npcBuildSO;
    public Vector2Int npcSpawnPos;
    public int requirePrestige;
    public int requireCrystal;
}