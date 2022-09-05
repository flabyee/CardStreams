using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class GradeAmount
{
    public int common;
    public int rare;
    public int epic;
    public int unique;
    public int legendary;
}

public class ShopController : MonoBehaviour
{
    // Seri
    [SerializeField] GameObject shopItemPrefab;
    // public

    // private
    List<ShopItemInfo> shopItemList = new List<ShopItemInfo>();
    // ui
    public CanvasGroup _cg;
    public RectTransform specialCardShopTrm;
    public RectTransform buildShopTrm;

    // 최소화 관련
    public CanvasGroup _minimizePanel;
    public TextMeshProUGUI miniMizeText;

    public TextMeshProUGUI rerollCostText;

    public Button rerollBtn;
    public Image lockBtnImage;

    // system
    private bool isMinimize;
    private bool isLock;

    [SerializeField]
    private int itemCount;  // 판매 갯수

    [SerializeField]
    List<GradeAmount> gradeAmount;
    [SerializeField]
    private int rerollCost;
    [SerializeField]
    private int mineUpgradeCost;
    [SerializeField]
    private List<int> upgradeCostList;
    private int shopGrade;

    // dict
    private List<BuildSO> buildList;
    private List<SpecialCardSO> specialList;

    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();
    private Dictionary<CardGrade, int> gradeToChance = new Dictionary<CardGrade, int>();


    // so
    public IntValue goldValue;
    public IntValue loopCountValue;
    public EventSO goldChangeEvnet;
    public EventSO nextTurnEvent;

    private void Awake()
    {
        shopGrade = -1;
        isLock = false;
        lockBtnImage.color = new Color(0, 0, 0, isLock ? 1f : 0.5f);

        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;
        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialList = specialListSO.specialCardList;

        for (int i = 0; i < 5; i++)
        {
            buildDict[(CardGrade)i] = new List<BuildData>();
            specialDict[(CardGrade)i] = new List<SpecialCardData>();
        }

        // 언락 되어있는 건물과 특수카드 dict에 넣기
        foreach (BuildData itemData in SaveFile.GetSaveData().buildDataList)
        {
            if (itemData.isUnlock == true && itemData.isUse == true)
            {
                BuildSO buildSO = buildList.Find((x) => x.id == itemData.id);

                buildDict[buildSO.grade].Add(itemData);
            }
        }

        foreach (SpecialCardData itemData in SaveFile.GetSaveData().speicialCardDataList)
        {
            if (itemData.isUnlock == true && itemData.isUse == true)
            {
                SpecialCardSO specialSO = specialList.Find((x) => x.id == itemData.id);
                specialDict[specialSO.grade].Add(itemData);
            }
        }
    }


    public void Show()
    {
        _cg.alpha = 1;
        _cg.blocksRaycasts = true;
        _cg.interactable = true;

        // 이전에 최소화했어도 다시 키기위해서
        isMinimize = false;
        _minimizePanel.alpha = 1;


        UpgradeShop();
        OnShop();
    }

    public void Hide()
    {
        _cg.alpha = 0;
        _cg.blocksRaycasts = false;
        _cg.interactable = false;
    }

    public void Minimize() // called by Button
    {
        isMinimize = !isMinimize;

        miniMizeText.text = isMinimize ? "확대" : "축소";

        _minimizePanel.alpha = isMinimize ? 0 : 1;
        _minimizePanel.interactable = isMinimize ? false : true;
        _minimizePanel.blocksRaycasts = isMinimize ? false : true;
        GameManager.Instance.blurController.SetActive(!isMinimize);
    }

    public void OnShop()
    {
        if (isLock == true)
        {
            isLock = false;
        }
        else
        {
            shopItemList.Clear();

            OnSpecialCardShop();
            OnBuildShop();
        }

        Renewal();
    }

    public void Renewal()
    {
        foreach (ShopItemInfo info in shopItemList)
        {
            info.priceText.color = info.price <= goldValue.RuntimeValue ? Color.white : Color.red;

            if(info.canBuy == false)
            {
                info.priceText.text = "X";
                info.priceText.color = Color.red;
            }
        }

        rerollCostText.text = $"리롤({rerollCost}원)";
        rerollCostText.color = rerollCost <= goldValue.RuntimeValue ? Color.white : Color.red;

        //if (upgradeCostList[shopGrade] != -1)
        //{
        //    upgradeCostText.text = $"상점 강화({upgradeCostList[shopGrade]})";
        //    upgradeCostText.color = upgradeCostList[shopGrade] <= goldValue.RuntimeValue ? Color.white : Color.red;
        //}
        //else
        //{
        //    upgradeCostText.text = $"강화 끝";
        //    upgradeCostText.color = Color.white;
        //}

        lockBtnImage.color = new Color(0, 0, 0, isLock ? 1f : 0.5f);

        rerollBtn.interactable = !isLock;
        // upgradeBtn.interactable = !isLock;
    }

    private void OnSpecialCardShop()
    {
        // 기존에 있던 상품 목록 제거
        foreach (Transform item in specialCardShopTrm)
        {
            item.gameObject.SetActive(false);
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

        for (int i = 0; i < itemCount; i++)
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

        for (int i = 0; i < itemCount; i++)
        {
            int randomChance = Random.Range(0, GetAllChance());
            if (randomChance < chance[0])
            {
                CreateBuildItem(CardGrade.Common, countDict[CardGrade.Common]++);
            }
            else if (randomChance < chance[1])
            {
                CreateBuildItem(CardGrade.Rare, countDict[CardGrade.Rare]++);
            }
            else if (randomChance < chance[2])
            {
                CreateBuildItem(CardGrade.Epic, countDict[CardGrade.Epic]++);
            }
            else if (randomChance < chance[3])
            {
                CreateBuildItem(CardGrade.Unique, countDict[CardGrade.Unique]++);
            }
            else if (randomChance < chance[4])
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
        if (cardType == CardType.Build)
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
        if (buildDict[grade].Count - 1 < i)
        {
            return;
        }

        BuildSO itemSO = buildList.Find((x) => x.id == buildDict[grade][i].id);


        if (itemSO != null)
        {
            GameObject shopItem = Instantiate(shopItemPrefab, buildShopTrm);
            ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

            shopItemList.Add(info);

            info.Init(itemSO.buildName, itemSO.accessPointList, itemSO.tooltip, itemSO.sprite, itemSO.grade, itemSO.price);

            info.button.onClick.AddListener(() =>
            {
                if(info.canBuy == true)
                {
                    SoundManager.Instance.PlaySFX(SFXType.BuyCard);
                    BuyBuild(itemSO, shopItem.transform.position);

                    info.canBuy = false;

                    Renewal();
                }
                
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
            GameObject shopItem = Instantiate(shopItemPrefab, specialCardShopTrm);
            ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

            shopItemList.Add(info);

            info.Init(itemSO.specialCardName, itemSO.targetTypeList, itemSO.targetBasicList, itemSO.tooltip, itemSO.sprite, itemSO.grade, itemSO.price);

            info.button.onClick.AddListener(() =>
            {
                if(info.canBuy == true)
                {
                    SoundManager.Instance.PlaySFX(SFXType.BuyCard);
                    BuySpecial(itemSO, shopItem.transform.position);

                    info.canBuy = false;

                    Renewal();
                }

            });
        }
        else
        {
            Debug.LogError("saveData에는 있는 id가 SO에 존재하지 않습니다");
        }

    }

    private void BuySpecial(SpecialCardSO specialCardSO, Vector3 pos)
    {
        if (specialCardSO.price <= goldValue.RuntimeValue)
        {
            EffectManager.Instance.GetBezierCardEffect(pos, specialCardSO.sprite, TargetType.Handle, () => { });

            goldValue.RuntimeValue -= specialCardSO.price;
            goldChangeEvnet.Occurred();

            GameManager.Instance.handleController.AddSpecial(specialCardSO.id);
        }
    }

    private void BuyBuild(BuildSO buildSO, Vector3 pos)
    {
        if (buildSO.price <= goldValue.RuntimeValue)
        {
            EffectManager.Instance.GetBezierCardEffect(pos, buildSO.sprite, TargetType.Handle, () => { });

            goldValue.RuntimeValue -= buildSO.price;
            goldChangeEvnet.Occurred();

            GameManager.Instance.handleController.AddBuild(buildSO.id);
        }
    }

    public void CloseShop()
    {
        Debug.Log("adsf");
        if (GameManager.Instance.canStartTurn == true)
        {
            Hide();

            nextTurnEvent.Occurred();
        }
    }

    public void SetChance()
    {
        //for(int i = 0; i < 5; i++)
        //{
        //    gradeToChance[(CardGrade)i] += chanceIncreaseAmountList[i];
        //}

        gradeToChance[CardGrade.Common] = gradeAmount[shopGrade].common;
        gradeToChance[CardGrade.Rare] = gradeAmount[shopGrade].rare;
        gradeToChance[CardGrade.Epic] = gradeAmount[shopGrade].epic;
        gradeToChance[CardGrade.Unique] = gradeAmount[shopGrade].unique;
        gradeToChance[CardGrade.Legendary] = gradeAmount[shopGrade].legendary;
    }

    public void OnClickReroll()
    {
        if (rerollCost <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= rerollCost;
            goldChangeEvnet.Occurred();

            OnShop();
        }
    }

    public void OnClickMineUpgrade()
    {
        if (mineUpgradeCost <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= mineUpgradeCost;
            goldChangeEvnet.Occurred();
            GameManager.Instance.mineLevel++;
        }
    }

    public void UpgradeShop()
    {
        if (shopGrade < 4)
        {
            shopGrade++;

            SetChance();

            if (shopGrade == 2 || shopGrade == 4)
            {
                Debug.Log("itemCount++ 이 있었으나 상점 연장점검으로인해 폐쇄되었습니다");
                // itemCount++;
            }
        }
    }

    public void OnClickLock()
    {
        isLock = !isLock;

        lockBtnImage.color = new Color(0, 0, 0, isLock ? 1f : 0.5f);

        rerollBtn.interactable = !isLock;
        // upgradeBtn.interactable = !isLock;
    }
}