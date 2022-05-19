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

    public List<RectTransform> specialCardStandList;    // �Ǹ��� ������ �÷��δ� ����
    public List<RectTransform> buildStandList;    // �Ǹ��� ������ �÷��δ� ����

    public int sellItemCount;

    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    private Dictionary<CardGrade, int> gradeToChance = new Dictionary<CardGrade, int>();

    private void Awake()
    {
        Hide();

        if(specialCardStandList.Count != sellItemCount)
        {
            //Debug.LogError("���� ������ �Ǹ� ������ �ǸŴ� ������ �ٸ��ϴ�");
        }

        SaveData saveData = DataManager.Instance.saveData;


        // unlock �� id list ��޺��� ������
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
        // ������ �ִ� ��ǰ ��� ����
        foreach (Transform item in specialCardShopTrm)
        {
            Destroy(item.gameObject);
        }

        // Ư��ī�� ����
        SaveData saveData = DataManager.Instance.saveData;

        // unlock �� id list �̱�
        List<SpecialCardData> unlockSpecialCardList = new List<SpecialCardData>();

        foreach (SpecialCardData itemData in saveData.speicialCardDataList)
        {
            if (itemData.isUnlock == true)
                unlockSpecialCardList.Add(itemData);
        }

        // �ű⼭ �������� 4��? �̱�
        for (int i = 0; i < unlockSpecialCardList.Count; i++)
        {
            int randomIndex = Random.Range(0, unlockSpecialCardList.Count);

            SpecialCardData temp = unlockSpecialCardList[i];
            unlockSpecialCardList[i] = unlockSpecialCardList[randomIndex];
            unlockSpecialCardList[randomIndex] = temp;
        }

        // �װ� ����
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
                Debug.LogError("saveData���� �ִ� id�� SO�� �������� �ʽ��ϴ�");
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

        
        // ���� ��� ���


        // ��ü ����Ʈ ����
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

        
        
        //// �װ� ����
        //for (int i = 0; i < sellItemCount; i++)
        //{
        //    // ���嵥���Ϳ��� �ִ� id�� SO�� �������
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
        //        Debug.LogError("saveData���� �ִ� id�� SO�� �������� �ʽ��ϴ�");
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