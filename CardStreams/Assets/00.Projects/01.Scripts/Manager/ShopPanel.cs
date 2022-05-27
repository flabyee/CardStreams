using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public GameObject shopPrefab;

    // ui
    public GameObject reducePanel;
    public RectTransform specialCardShopTrm;
    public RectTransform buildShopTrm;


    // system
    public List<int> chanceFirstAmountList;
    public List<int> chanceIncreaseAmountList;

    private bool isReduce;

    // dict
    private List<BuildSO> buildList;
    private List<SpecialCardSO> specialList;

    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();
    private Dictionary<CardGrade, int> gradeToChance = new Dictionary<CardGrade, int>();


    // so
    public IntValue goldValue;
    public IntValue turnCountValue;
    public EventSO goldChangeEvnet;
    public EventSO nextTurnEvent;

    private void Awake()
    {
        SaveData saveData = SaveSystem.Load();

        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;
        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialList = specialListSO.specialCardList;

        for (int i = 0; i < 5; i++)
        {
            buildDict[(CardGrade)i] = new List<BuildData>();
            specialDict[(CardGrade)i] = new List<SpecialCardData>();
        }

        foreach (BuildData itemData in saveData.buildDataList)
        {
            if (itemData.isUnlock == true)
            {
                BuildSO buildSO = buildList.Find((x) => x.id == itemData.id);

                buildDict[buildSO.grade].Add(itemData);
            }
        }

        foreach (SpecialCardData itemData in saveData.speicialCardDataList)
        {
            if (itemData.isUnlock == true)
            {
                SpecialCardSO specialSO = specialList.Find((x) => x.id == itemData.id);

                specialDict[specialSO.grade].Add(itemData);
            }
        }

        for(int i = 0; i < 5; i++)
        {
            gradeToChance[(CardGrade)i] = chanceFirstAmountList[i];
        }
    }

    private void Start()
    {
        Debug.Log("Start");
        Show();
    }



    public void Show()
    {
        gameObject.SetActive(true);
        reducePanel.SetActive(true);
        isReduce = false;

        OnShop();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Reduce()
    {
        reducePanel.SetActive(isReduce);
        isReduce = !isReduce;
    }

    public void OnShop()
    {
        SetChance();

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

        Shuffle(CardType.Special);


        Dictionary<CardGrade, int> countDict = new Dictionary<CardGrade, int>();
        countDict[CardGrade.Common] = 0;
        countDict[CardGrade.Rare] = 0;
        countDict[CardGrade.Epic] = 0;
        countDict[CardGrade.Unique] = 0;
        countDict[CardGrade.Legendary] = 0;

        int[] chance = new int[5];
        chance[0] = gradeToChance[CardGrade.Common] > 0 ? gradeToChance[CardGrade.Common] : 0;
        chance[1] = chance[0] + (gradeToChance[CardGrade.Rare] > 0 ? gradeToChance[CardGrade.Rare] : 0);
        chance[2] = chance[1] + (gradeToChance[CardGrade.Epic] > 0 ? gradeToChance[CardGrade.Epic] : 0);
        chance[3] = chance[2] + (gradeToChance[CardGrade.Unique] > 0 ? gradeToChance[CardGrade.Unique] : 0);
        chance[4] = chance[3] + (gradeToChance[CardGrade.Legendary] > 0 ? gradeToChance[CardGrade.Legendary] : 0);

        for (int i = 0; i < 10; i++)
        {

            int randomChance = Random.Range(0, GetAllChance());
            if (randomChance < chance[0])
            {
                CreateSpecialCardItem(CardGrade.Common, countDict[CardGrade.Common]++);
            }
            else if (randomChance < chance[1])
            {
                CreateSpecialCardItem(CardGrade.Rare, countDict[CardGrade.Rare]++);
            }
            else if (randomChance < chance[2])
            {
                CreateSpecialCardItem(CardGrade.Epic, countDict[CardGrade.Epic]++);
            }
            else if (randomChance < chance[3])
            {
                CreateSpecialCardItem(CardGrade.Unique, countDict[CardGrade.Unique]++);
            }
            else if (randomChance < chance[4])
            {
                CreateSpecialCardItem(CardGrade.Legendary, countDict[CardGrade.Legendary]++);
            }
            else
            {
                Debug.LogError("chance 설정이 잘못 됨");
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

        Shuffle(CardType.Build);


        Dictionary<CardGrade, int> countDict = new Dictionary<CardGrade, int>();
        countDict[CardGrade.Common] = 0;
        countDict[CardGrade.Rare] = 0;
        countDict[CardGrade.Epic] = 0;
        countDict[CardGrade.Unique] = 0;
        countDict[CardGrade.Legendary] = 0;

        int[] chance = new int[5];
        chance[0] = gradeToChance[CardGrade.Common] > 0 ? gradeToChance[CardGrade.Common] : 0;
        chance[1] = chance[0] + (gradeToChance[CardGrade.Rare] > 0 ? gradeToChance[CardGrade.Rare] : 0);
        chance[2] = chance[1] + (gradeToChance[CardGrade.Epic] > 0 ? gradeToChance[CardGrade.Epic] : 0);
        chance[3] = chance[2] + (gradeToChance[CardGrade.Unique] > 0 ? gradeToChance[CardGrade.Unique] : 0);
        chance[4] = chance[3] + (gradeToChance[CardGrade.Legendary] > 0 ? gradeToChance[CardGrade.Legendary] : 0);
        
        for (int i = 0; i < 10; i++)
        {
            int randomChance = Random.Range(0, GetAllChance());
            if (randomChance < chance[0])
            {
                CreateBuildItem(CardGrade.Common, countDict[CardGrade.Common]++);
            }
            else if(randomChance < chance[1])
            {
                CreateBuildItem(CardGrade.Rare, countDict[CardGrade.Rare]++);
            }
            else if(randomChance < chance[2])
            {
                CreateBuildItem(CardGrade.Epic, countDict[CardGrade.Epic]++);
            }
            else if(randomChance < chance[3])
            {
                CreateBuildItem(CardGrade.Unique, countDict[CardGrade.Unique]++);
            }
            else if(randomChance < chance[4])
            {
                CreateBuildItem(CardGrade.Legendary, countDict[CardGrade.Legendary]++);
            }
            else
            {
                Debug.LogError("chance 설정이 잘못 됨");
            }
        }
    }

    private void Shuffle(CardType cardType)
    {
        if (cardType == CardType.Special)
        {
            foreach (var list in specialDict.Values)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    int randomIndex = Random.Range(0, list.Count);

                    SpecialCardData temp = list[i];
                    list[i] = list[randomIndex];
                    list[randomIndex] = temp;
                }
            }
        }
        if(cardType == CardType.Build)
        {
            foreach (var list in buildDict.Values)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    int randomIndex = Random.Range(0, list.Count);

                    BuildData temp = list[i];
                    list[i] = list[randomIndex];
                    list[randomIndex] = temp;
                }
            }
        }
    }

    private int GetAllChance()
    {
        int allChance = 0;
        foreach (int chance in gradeToChance.Values)
        {
            allChance += chance > 0 ? chance : 0;
        }

        return allChance;
    }

    private void CreateBuildItem(CardGrade grade, int i)
    {
        if(buildDict[grade].Count - 1 < i)
        {
            return;
        }

        BuildSO itemSO = buildList.Find((x) => x.id == buildDict[grade][i].id);


        if (itemSO != null)
        {
            GameObject shopItem = Instantiate(shopPrefab, buildShopTrm);
            ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

            info.Init(itemSO.buildName, itemSO.accessPointList, itemSO.tooltip, itemSO.sprite, itemSO.grade, itemSO.price);

            info.button.onClick.AddListener(() =>
            {
                BuyBuild(itemSO);

                //OnShop();
            });
        }
        else
        {
            Debug.LogError("saveData에는 있는 id가 SO에 존재하지 않습니다");
        }
    }

    private void CreateSpecialCardItem(CardGrade grade, int i)
    {
        if (specialDict[grade].Count - 1 < i)
        {
            return;
        }

        SpecialCardSO itemSO = specialList.Find((x) => x.id == specialDict[grade][i].id);


        if (itemSO != null)
        {
            GameObject shopItem = Instantiate(shopPrefab, specialCardShopTrm);
            ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

            info.Init(itemSO.specialCardName, itemSO.targetTypeList, itemSO.tooltip, itemSO.sprite, itemSO.grade, itemSO.price);

            info.button.onClick.AddListener(() =>
            {
                BuySpecial(itemSO);

                //OnShop();
            });
        }
        else
        {
            Debug.LogError("saveData에는 있는 id가 SO에 존재하지 않습니다");
        }
    }

    private void BuySpecial(SpecialCardSO specialCardSO)
    {
        if (specialCardSO.price <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= specialCardSO.price;
            goldChangeEvnet.Occurred();

            SaveData saveData = SaveSystem.Load();
            saveData.speicialCardDataList[specialCardSO.id].haveAmount++;
            SaveSystem.Save(saveData);
        }
    }

    private void BuyBuild(BuildSO buildSO)
    {
        if (buildSO.price <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= buildSO.price;
            goldChangeEvnet.Occurred();

            SaveData saveData = SaveSystem.Load();
            saveData.buildDataList[buildSO.id].haveAmount++;
            SaveSystem.Save(saveData);
        }
    }

    public void CloseShop()
    {
        Debug.Log("adsf");
        if(GameManager.Instance.canStartTurn == true)
        {
            Hide();

            nextTurnEvent.Occurred();
        }
    }

    private void SetChance()
    {
        for(int i = 0; i < 5; i++)
        {
            gradeToChance[(CardGrade)i] += chanceIncreaseAmountList[i];
        }

        for(int i = 0; i < 5; i++)
        {
            // Debug.Log((CardGrade)i + " : " + (float)gradeToChance[(CardGrade)i] / GetAllChance() * 100f);
        }
    }
}