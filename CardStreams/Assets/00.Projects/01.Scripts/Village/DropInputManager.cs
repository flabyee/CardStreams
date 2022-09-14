using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInputManager : MonoBehaviour
{
    public static DropInputManager Instance;
    public VillageShop villageShop; 

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            VillageShopItem.buildPos = new Vector2Int(5, 2);
        }
    }

    public void TargetingBuildRect()
    {
        villageShop.OpenMenu();
    }
}
