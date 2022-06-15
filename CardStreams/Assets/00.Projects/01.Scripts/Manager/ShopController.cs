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
    public CanvasGroup _reduceCG;
    public RectTransform specialCardShopTrm;
    public RectTransform buildShopTrm;
    public GameObject blurObj;

    public RectTransform handleTrm;
    public RectTransform hoverTrm;

    public TextMeshProUGUI rerollCostText;
    public TextMeshProUGUI upgradeCostText;

    // system
    private bool isMinimize;

    [SerializeField]
    private int itemCount;  // �Ǹ� ����

    [SerializeField] 
    List<GradeAmount> gradeAmount;
    [SerializeField]
    private int rerollCost;
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
    public IntValue turnCountValue;
    public EventSO goldChangeEvnet;
    public EventSO nextTurnEvent;

    private void Awake()
    {
        shopGrade = 0;
        SetChance();

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

    }


    public void Show()
    {
        _cg.alpha = 1;
        _cg.blocksRaycasts = true;
        _cg.interactable = true;

        // ������ �ּ�ȭ�߾ �ٽ� Ű�����ؼ�
        isMinimize = false;
        _reduceCG.alpha = 1;

        blurObj.SetActive(true);

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

        _reduceCG.alpha = isMinimize ? 0 : 1;
        _reduceCG.interactable = isMinimize ? false : true;
        _reduceCG.blocksRaycasts = isMinimize ? false : true;
        blurObj.SetActive(!isMinimize);
    }

    public void OnShop()
    {
        shopItemList.Clear();

        OnSpecialCardShop();
        OnBuildShop();

        Renewal();
    }

    public void Renewal()
    {
        foreach(ShopItemInfo info in shopItemList)
        {
            info.priceText.color = info.price <= goldValue.RuntimeValue ? Color.white : Color.red;
        }

        rerollCostText.text = $"����({rerollCost})";
        rerollCostText.color = rerollCost <= goldValue.RuntimeValue ? Color.white : Color.red;

        if (upgradeCostList[shopGrade] != -1)
        {
            upgradeCostText.text = $"���� ��ȭ({upgradeCostList[shopGrade]})";
            upgradeCostText.color = upgradeCostList[shopGrade] <= goldValue.RuntimeValue ? Color.white : Color.red;
        }
        else
        {
            upgradeCostText.text = $"��ȭ ��";
            upgradeCostText.color = Color.white;
        }
    }

    private void OnSpecialCardShop()
    {
        // ������ �ִ� ��ǰ ��� ����
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
                Debug.LogError("chance ������ �߸� ��");
            }
        }
    }

    private void OnBuildShop()
    {
        // ������ �ִ� ��ǰ ��� ����
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
                Debug.LogError("chance ������ �߸� ��");
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
            GameObject shopItem = Instantiate(shopItemPrefab, buildShopTrm);
            ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

            shopItemList.Add(info);

            info.Init(itemSO.buildName, itemSO.accessPointList, itemSO.tooltip, itemSO.sprite, itemSO.grade, itemSO.price);

            info.button.onClick.AddListener(() =>
            {
                BuyBuild(itemSO, shopItem.transform.position);

                Renewal();

                //OnShop();
            });
        }
        else
        {
            Debug.LogError("saveData���� �ִ� id�� SO�� �������� �ʽ��ϴ�");
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
                BuySpecial(itemSO, shopItem.transform.position);

                Renewal();

                //OnShop();
            });
        }
        else
        {
            Debug.LogError("saveData���� �ִ� id�� SO�� �������� �ʽ��ϴ�");
        }
    }

    private void BuySpecial(SpecialCardSO specialCardSO, Vector3 pos)
    {
        if (specialCardSO.price <= goldValue.RuntimeValue)
        {
            EffectManager.Instance.GetBezierCardEffect(pos, specialCardSO.sprite, specialCardSO.id, () => GameManager.Instance.handleController.DrawSpecialCard(specialCardSO.id));

            goldValue.RuntimeValue -= specialCardSO.price;
            goldChangeEvnet.Occurred();
        }
    }

    private void BuyBuild(BuildSO buildSO, Vector3 pos)
    {
        if (buildSO.price <= goldValue.RuntimeValue)
        {
            EffectManager.Instance.GetBezierCardEffect(pos, buildSO.sprite, buildSO.id, () => GameManager.Instance.handleController.DrawBuildCard(buildSO.id));

            goldValue.RuntimeValue -= buildSO.price;
            goldChangeEvnet.Occurred();
        }
    }

    IEnumerator Delay(System.Action action, float t)
    {
        yield return new WaitForSeconds(t);

        action?.Invoke();
    }

    private IEnumerator ActiveFalseCor(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);

        obj.SetActive(false);
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
        if(rerollCost <= goldValue.RuntimeValue)
        {
            goldValue.RuntimeValue -= rerollCost;
            goldChangeEvnet.Occurred();

            OnShop();
        }
    }

    public void OnClickShopUpgrade()
    {
        if (upgradeCostList[shopGrade] <= goldValue.RuntimeValue && shopGrade < upgradeCostList.Count - 1)
        {
            goldValue.RuntimeValue -= upgradeCostList[shopGrade];
            goldChangeEvnet.Occurred();

            shopGrade++;

            SetChance();

            if(shopGrade == 2 || shopGrade == 4)
                itemCount++;

            OnShop();
        }
    }
}