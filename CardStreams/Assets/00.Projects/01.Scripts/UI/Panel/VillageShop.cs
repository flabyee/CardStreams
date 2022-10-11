using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class VillageShop : MonoBehaviour
{
    [SerializeField] BuildListSO villageBuildListSO;
    public BuildingInfoPanel info;

    [SerializeField] RectTransform panelShowTrm; // 패널이 on되고나서 이동할 위치
    [SerializeField] RectTransform panelHideTrm; // 패널이 on되고나서 이동할 위치

    private RectTransform _rectTrm;
    private List<VillageShopItem> shopItems = new List<VillageShopItem>();

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
            //// 열기
            //info.Show();

            _rectTrm.DOAnchorPosX(panelShowTrm.anchoredPosition.x, 1.0f);
            DropInputManager.Instance.SetActiveHighlight(true);
            foreach (VillageShopItem item in shopItems)
            {
                item.SetInteractable(true);
            }
        }
        else
        {
            // 닫기
            info.Hide();

            _rectTrm.DOAnchorPosX(panelHideTrm.anchoredPosition.x, 1.0f);
            DropInputManager.Instance.SetActiveHighlight(false);
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
