using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInputManager : MonoBehaviour
{
    public static DropInputManager Instance;
    public VillageShop villageShop;
    public GameObject highlight;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
    
    public void TargetingBuildRect(Vector3 pos)
    {
        villageShop.OnOffMenu();
        highlight.transform.position = pos; // 켜주는건 villageShop에서
    }

    public void SetActiveHighlight(bool isActive)
    {
        highlight.SetActive(isActive);
    }
}
