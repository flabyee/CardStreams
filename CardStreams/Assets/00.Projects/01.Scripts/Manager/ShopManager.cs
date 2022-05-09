using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPrefab;

    public GameObject shopPanel;
    public RectTransform specialCardShopTrm;
    public RectTransform buildShopTrm;

    public EventSO nextTurnEvent;

    public IntValue goldValue;
    public EventSO goldChangeEvnet;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        
    }

    public void Show()
    {
        shopPanel.SetActive(true);

        OnShop();
    }

    public void Hide()
    {
        shopPanel.SetActive(false);
    }

    private void OnShop()
    {
        foreach(Transform item in specialCardShopTrm)
        {
            Destroy(item.gameObject);
        }
        foreach(Transform item in buildShopTrm)
        {
            Destroy(item.gameObject);
        }

        // 특수카드 상점
        Dictionary<int, int> haveSpecialDic = DataManager.Instance.GetHaveSpecialCardDic();
        List<SpecialCardSO> allSpecialCardSOList = DataManager.Instance.GetSpecialSOList();
        foreach (SpecialCardSO itemSO in allSpecialCardSOList)
        {
            int itemID = itemSO.id;
            GameObject shopItem = Instantiate(shopPrefab, specialCardShopTrm);
            ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

            // dic에 있으면 dic[id], 없으면 0 (갯수)
            if(haveSpecialDic.ContainsKey(itemID))
            {
                info.Init(itemSO.specialCardName, itemSO.targetTypeList, itemSO.tooltip, itemSO.sprite, haveSpecialDic[itemID], itemSO.price);
            }
            else
            {
                info.Init(itemSO.specialCardName, itemSO.targetTypeList, itemSO.tooltip, itemSO.sprite, 0, itemSO.price);
            }

            info.button.onClick.AddListener(() =>
            {
                BuySpecial(itemSO);

                OnShop();
            });

        }

        // 건물 상점
        Dictionary<int, int> haveBuildDic = DataManager.Instance.GetHaveBuildDic();
        List<BuildSO> allBuildSOList = DataManager.Instance.GetBuildSOList();
        foreach (BuildSO itemSO in allBuildSOList)
        {
            int itemID = itemSO.id;
            GameObject shopItem = Instantiate(shopPrefab, buildShopTrm);
            ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

            // dic에 있으면 dic[id], 없으면 0 (갯수)
            if(haveBuildDic.ContainsKey(itemID))
            {
                info.Init(itemSO.buildName, null, itemSO.tooltip, itemSO.sprite, haveBuildDic[itemID], itemSO.price);
            }
            else
            {
                info.Init(itemSO.buildName, null, itemSO.tooltip, itemSO.sprite, 0, itemSO.price);
            }

            info.button.onClick.AddListener(() =>
            {
                BuyBuild(itemSO);

                OnShop();
            });
        }
    }

    private void BuySpecial(SpecialCardSO specialCardSO)
    {
        if (specialCardSO.price <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= specialCardSO.price;
            goldChangeEvnet.Occurred();

            DataManager.Instance.AddSpecialCard(specialCardSO.id);

            //DataManager.Instance.Save();
        }
    }

    private void BuyBuild(BuildSO buildSO)
    {
        if (buildSO.price <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= buildSO.price;
            goldChangeEvnet.Occurred();

            DataManager.Instance.AddBuild(buildSO.id);

            //DataManager.Instance.Save();
        }
    }

    public void NextTurn()
    {
        Hide();

        nextTurnEvent.Occurred();
    }
}