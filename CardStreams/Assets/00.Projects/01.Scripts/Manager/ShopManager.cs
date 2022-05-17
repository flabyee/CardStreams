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

    public List<RectTransform> specialCardStandList;    // �Ǹ��� ������ �÷��δ� ����
    public List<RectTransform> buildStandList;    // �Ǹ��� ������ �÷��δ� ����

    public int sellItemCount;


    private void Awake()
    {
        Hide();

        if(specialCardStandList.Count != sellItemCount)
        {
            //Debug.LogError("���� ������ �Ǹ� ������ �ǸŴ� ������ �ٸ��ϴ�");
        }
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
        for (int i = 0; i < sellItemCount; i++)
        {
            SpecialCardSO itemSO = DataManager.Instance.GetSpecialCardSO(unlockSpecialCardList[i].id);

            if(itemSO != null)
            {
                GameObject shopItem = Instantiate(shopPrefab, specialCardShopTrm);
                ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

                info.Init(itemSO.specialCardName, itemSO.targetTypeList, itemSO.tooltip, itemSO.sprite, unlockSpecialCardList[i].haveAmount, itemSO.price);

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

        // �ǹ� ����
        SaveData saveData = DataManager.Instance.saveData;

        // unlock �� id list �̱�
        List<BuildData> unlockBuildList = new List<BuildData>();

        foreach (BuildData itemData in saveData.buildDataList)
        {
            if (itemData.isUnlock == true)
                unlockBuildList.Add(itemData);
        }

        // �ű⼭ �������� 4��? �̱�
        for (int i = 0; i < unlockBuildList.Count; i++)
        {
            int randomIndex = Random.Range(0, unlockBuildList.Count);

            BuildData temp = unlockBuildList[i];
            unlockBuildList[i] = unlockBuildList[randomIndex];
            unlockBuildList[randomIndex] = temp;
        }

        // �װ� ����
        for (int i = 0; i < sellItemCount; i++)
        {
            // ���嵥���Ϳ��� �ִ� id�� SO�� �������
            BuildSO itemSO = DataManager.Instance.GetBuildSO(unlockBuildList[i].id);

            if(itemSO != null)
            {
                GameObject shopItem = Instantiate(shopPrefab, buildShopTrm);
                ShopItemInfo info = shopItem.GetComponent<ShopItemInfo>();

                info.Init(itemSO.buildName, itemSO.accessPointList, itemSO.tooltip, itemSO.sprite, unlockBuildList[i].haveAmount, itemSO.price);

                info.button.onClick.AddListener(() =>
                {
                    BuyBuild(itemSO);

                    //OnShop();
                });
            }
            else
            {
                Debug.LogError("saveData���� �ִ� id�� SO�� �������� �ʽ��ϴ�");
            }
        }
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