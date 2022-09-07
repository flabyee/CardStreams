using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildLauncher : MonoBehaviour
{
    public int spawnX;
    public int spawnY;
    public BuildSO launchBuildSO;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            
            RectTransform rectTrm = VillageMapManager.Instance.GetMapRectTrm(spawnY, spawnX);
            BuildCard building = CardPoolManager.Instance.GetBuildCard(rectTrm).GetComponent<BuildCard>();
            building.transform.position = rectTrm.position;

            building.Init(launchBuildSO);
            building.VillageBuildDrop(new Vector2(spawnY, spawnX));

            CardPower cardPower = building.GetComponent<CardPower>();
            cardPower.backImage.color = Color.magenta;
            cardPower.OnField();
        }
    }
}
