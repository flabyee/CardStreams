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
    public IntValue turnCountValue;
    public EventSO goldChangeEvnet;

    public List<RectTransform> specialCardStandList;    // 판매할 아이템 올려두는 공간
    public List<RectTransform> buildStandList;    // 판매할 아이템 올려두는 공간

    public int sellItemCount;

    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    private Dictionary<CardGrade, int> gradeToChance = new Dictionary<CardGrade, int>();

    private void Awake()
    {
        Hide();

        if(specialCardStandList.Count != sellItemCount)
        {
            //Debug.LogError("상점 아이템 판매 갯수와 판매대 갯수가 다릅니다");
        }

        SaveData saveData = DataManager.Instance.saveData;


        // unlock 된 id list 등급별로 나누기
        buildDict[CardGrade.Common] = new List<BuildData>();
        buildDict[CardGrade.Rare] = new List<BuildData>();
        buildDict[CardGrade.Epic] = new List<BuildData>();
        buildDict[CardGrade.Unique] = new List<BuildData>();
        buildDict[CardGrade.Legendary] = new List<BuildData>();

        specialDict[CardGrade.Common] = new List<SpecialCardData>();
        specialDict[CardGrade.Rare] = new List<SpecialCardData>();
        specialDict[CardGrade.Epic] = new List<SpecialCardData>();
        specialDict[CardGrade.Unique] = new List<SpecialCardData>();
        specialDict[CardGrade.Legendary] = new List<SpecialCardData>();


        foreach (BuildData itemData in saveData.buildDataList)
        {
            if (itemData.isUnlock == true)
            {
                BuildSO buildSO = DataManager.Instance.GetBuildSO(itemData.id);

                buildDict[buildSO.grade].Add(itemData);
            }
        }

        foreach (SpecialCardData itemData in saveData.speicialCardDataList)
        {
            if (itemData.isUnlock == true)
            {
                SpecialCardSO specialSO = DataManager.Instance.GetSpecialCardSO(itemData.id);

                specialDict[specialSO.grade].Add(itemData);
            }
        }
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

    public void OnShop()
    {



        OnSpecialCardShop();
        OnBuildShop();
    }

    private void OnSpecialCardShop()
    {
        // 기존에 있던 상품 목록 제거
        foreach (Transform item in specialCardShopTrm)
        {
            Destroy(item.gameObject);
        }

        // 특수카드 상점
        SaveData saveData = DataManager.Instance.saveData;

        // unlock 된 id list 뽑기
        List<SpecialCardData> unlockSpecialCardList = new List<SpecialCardData>();

        foreach (SpecialCardData itemData in saveData.speicialCardDataList)
        {
            if (itemData.isUnlock == true)
                unlockSpecialCardList.Add(itemData);
        }

        // 거기서 랜덤으로 4개? 뽑기
        for (int i = 0; i < unlockSpecialCardList.Count; i++)
        {
            int randomIndex = Random.Range(0, unlockSpecialCardList.Count);

            SpecialCardData temp = unlockSpecialCardList[i];
            unlockSpecialCardList[i] = unlockSpecialCardList[randomIndex];
            unlockSpecialCardList[randomIndex] = temp;
        }

        // 그거 생성
        for (int i = 0; i < sellItemCount * 2; i++)
        {
            SpecialCardSO itemSO = DataManager.Instance.GetSpecialCardSO(unlockSpecialCardList[i].id);

            if(itemSO != null)
            {
                GameObject shopItem = Instantiate(shopPrefab, specialCardShopTrm);
                ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

                info.Init(itemSO.specialCardName, itemSO.targetTypeList, itemSO.tooltip, itemSO.sprite, itemSO.grade, itemSO.price);

                info.button.onClick.AddListener(() =>
                {
                    BuySpecial(itemSO);

                    //OnSpecialCardShop();
                });
            }
            else
            {
                Debug.LogError("saveData에는 있는 id가 SO에 존재하지 않습니다");
            }
        }
    }

    private void OnBuildShop()
    {
        // 기존에 있던 상품 목록 제거
        foreach (Transform item in buildShopTrm)
        {
            Destroy(item.gameObject);
        }

        
        // 랜덤 등급 얻기


        // 전체 리스트 섞기
        foreach(var list in buildDict.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(0, list.Count);

                BuildData temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        
        
        //// 그거 생성
        //for (int i = 0; i < sellItemCount; i++)
        //{
        //    // 저장데이터에는 있는 id가 SO에 없을까봐
        //    BuildSO itemSO = DataManager.Instance.GetBuildSO(unlockBuildList[i].id);

        //    if(itemSO != null)
        //    {
        //        GameObject shopItem = Instantiate(shopPrefab, buildShopTrm);
        //        ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

        //        info.Init(itemSO.buildName, itemSO.accessPointList, itemSO.tooltip, itemSO.sprite, itemSO.grade, itemSO.price);

        //        info.button.onClick.AddListener(() =>
        //        {
        //            BuyBuild(itemSO);

        //            //OnShop();
        //        });
        //    }
        //    else
        //    {
        //        Debug.LogError("saveData에는 있는 id가 SO에 존재하지 않습니다");
        //    }
        //}
    }

    private List<int> GetGradeList()
    {
        return null;
        //List<int>
        //switch(turnCountValue.RuntimeValue)
        //{
        //    case 0:

        //}
    }

    private void BuySpecial(SpecialCardSO specialCardSO)
    {
        if (specialCardSO.price <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= specialCardSO.price;
            goldChangeEvnet.Occurred();

            SaveData saveData = DataManager.Instance.saveData;
            saveData.speicialCardDataList[specialCardSO.id].haveAmount++;

            //DataManager.Instance.Save();
        }
    }

    private void BuyBuild(BuildSO buildSO)
    {
        if (buildSO.price <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= buildSO.price;
            goldChangeEvnet.Occurred();

            SaveData saveData = DataManager.Instance.saveData;
            saveData.buildDataList[buildSO.id].haveAmount++;

            //DataManager.Instance.Save();
        }
    }

    public void NextTurn()
    {
        if(GameManager.Instance.canStartTurn == true)
        {
            Hide();

            nextTurnEvent.Occurred();
        }
    }
}