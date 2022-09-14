using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageShopItem : MonoBehaviour
{
    public VillageBuildSO buildItemSO;
    public static Vector2Int buildPos;

    public void Buy()
    {
        // buildItemSO¸¦ ±ò±â
        CreateNpc();

        // Å©¸®½ºÅ» ¼Ò¸ð
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void CreateNpc()
    {
        RectTransform rectTrm = VillageMapManager.Instance.GetMapRectTrm(buildPos.y, buildPos.x);
        BuildCard building = CardPoolManager.Instance.GetBuildCard(rectTrm).GetComponent<BuildCard>();
        building.transform.position = rectTrm.position;

        building.Init(buildItemSO);
        building.VillageBuildDrop(new Vector2(buildPos.x, buildPos.y));

        CardPower cardPower = building.GetComponent<CardPower>();
        cardPower.backImage.color = Color.magenta;
        cardPower.OnField();
    }
}
