using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageEquipHouse", menuName = "ScriptableObject/Build/Village/EquipHouse")]
public class VillageEquipHouseSO : VillageBuildSO
{
    public EventSO openVillagMenuEvent;

    public override void AccessPlayer(Player player)
    {
        MenuManager.Instance.OpenMenu(MenuType.Equip);
        openVillagMenuEvent.Occurred();
    }
}
