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
    
    public void TargetingBuildRect()
    {
        villageShop.OnOffMenu();
    }
}
