using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class VillageShop : MonoBehaviour
{
    [SerializeField] BuildListSO villageBuildListSO;

    private RectTransform _rectTrm;
    private List<VillageShopItem> shopItems = new List<VillageShopItem>();

    private float openPosX = -30;
    private float closePosX = 470;
    private bool isOpen = false;


    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
    }

    private void Start()
    {
        shopItems.AddRange(GetComponentsInChildren<VillageShopItem>());
        for (int i = 0; i < villageBuildListSO.buildList.Count; i++)
        {
            shopItems[i].Init(villageBuildListSO.buildList[i]);
        }
    }

    public void OnOffMenu()
    {
        // 닫혀있었으면 열고 열려있었으면 닫고
        isOpen = !isOpen;

        if (isOpen)
        {
            // 열기
            _rectTrm.DOAnchorPosX(openPosX, 1.0f);
            foreach (VillageShopItem item in shopItems)
            {
                item.SetInteractable(true);
            }
        }
        else
        {
            // 닫기
            _rectTrm.DOAnchorPosX(closePosX, 1.0f);
            foreach (VillageShopItem item in shopItems)
            {
                item.SetInteractable(false);
            }
        }
    }

    private void OnDestroy()
    {
        _rectTrm.DOKill();
    }
}
