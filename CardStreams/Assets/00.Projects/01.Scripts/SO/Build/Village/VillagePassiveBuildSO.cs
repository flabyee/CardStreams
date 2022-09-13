using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VillagePassiveBuild", menuName = "ScriptableObject/Build/Village/Passive")]
public class VillagePassiveBuildSO : VillageBuildSO
{
    public BuffSO passiveSO;

    public override void AccessPlayer(Player player)
    {
        var villagePlayer = player as VillagePlayer;
        if (villagePlayer != null)
        {
            villagePlayer.passiveSystem.AddPassive(passiveSO);
        }
    }
}
