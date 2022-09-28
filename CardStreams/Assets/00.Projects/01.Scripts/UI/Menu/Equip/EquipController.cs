using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipController : MonoBehaviour
{
    private Dictionary<CardGrade, List<BuildCardData>> buildDict = new Dictionary<CardGrade, List<BuildCardData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    // ���� ������ ���� ����Ʈ
    private List<RemoveBuildCard> removeBuildList = new List<RemoveBuildCard>();
    private List<RemoveSpecialCard> removeSpecialList = new List<RemoveSpecialCard>();

    private List<BuildSO> buildList;
    private List<SpecialCardSO> specialList;

    private SaveData saveData;

    private int maxRemoveCount;
    private int removeCount;

    [Header("UI")]
    public RectTransform buildScrollTrm;
    public RectTransform specialScrollTrm;

    public RemoveBuildCard buildCardPrefab;
    public RemoveSpecialCard specialCardPrefab;

    public TextMeshProUGUI countText;

    public RectTransform rightListTrm;
    public GameObject rightListPrefab;

    public TextMeshProUGUI goldText;

    public System.Action UpgardeRemoveCountEvent;

    private void Awake()
    {
        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;
        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialList = specialListSO.specialCardList;
    }

    public void OnEquip()
    {
        saveData = SaveFile.GetSaveData();

        maxRemoveCount = saveData.maxRemoveCount;

        removeCount = 0;

        buildDict.Clear();
        specialDict.Clear();

        foreach(RectTransform item in buildScrollTrm)
        {
            Destroy(item.gameObject);
        }    
        foreach(RectTransform item in specialScrollTrm)
        {
            Destroy(item.gameObject);
        }
        
        buildScrollTrm.sizeDelta = new Vector2(0, 325f);
        specialScrollTrm.sizeDelta = new Vector2(0, 325f);

        for (int i = 0; i < 5; i++)
        {
            buildDict[(CardGrade)i] = new List<BuildCardData>();
            specialDict[(CardGrade)i] = new List<SpecialCardData>();
        }

        // ��� �Ǿ��ִ� �ǹ��� Ư��ī�� dict�� �ֱ�
        foreach (BuildCardData itemData in saveData.buildDataList)
        {
            if (itemData.isUnlock == true)
            {
                BuildSO buildSO = buildList.Find((x) => x.id == itemData.id);

                buildDict[buildSO.grade].Add(itemData);

                if(itemData.isUse == false)
                {
                    removeCount++;
                }
            }
        }

        foreach (SpecialCardData itemData in saveData.speicialCardDataList)
        {
            if (itemData.isUnlock == true)
            {
                SpecialCardSO specialSO = specialList.Find((x) => x.id == itemData.id);

                specialDict[specialSO.grade].Add(itemData);

                if (itemData.isUse == false)
                {
                    removeCount++;
                }
            }
        }

        if(saveData.maxRemoveCount < removeCount)
        {
            Debug.LogError("�ִ� ���� ���� ���� ������ ������ �� �����ϴ�");
        }

        CreateCard();

        ApplyUI();
    }

    public void Close()
    {
        foreach (RectTransform item in rightListTrm)
        {
            Destroy(item.gameObject);
        }

        foreach (RectTransform item in buildScrollTrm)
        {
            Destroy(item.gameObject);
        }
        foreach (RectTransform item in specialScrollTrm)
        {
            Destroy(item.gameObject);
        }

        removeBuildList.Clear();
        removeSpecialList.Clear();
    }

    public void CreateCard()
    {
        int count = 0;
        bool b = false;
        // �ǹ� removeCard ����
        foreach(List<BuildCardData> buildDataList in buildDict.Values)
        {
            foreach(BuildCardData buildData in buildDataList)
            {
                if(b == true)
                {
                    b = false;
                    buildScrollTrm.sizeDelta += new Vector2(0, 325f);
                }
                RemoveBuildCard build = Instantiate(buildCardPrefab, buildScrollTrm);

                build.Init(DataManager.Instance.GetBuildSO(buildData.id), buildData.isUse);
                build.button.onClick.AddListener(() =>
                {
                    int closerID = buildData.id;

                    OnClickBuildRemove(closerID, build);
                });

                count++;
                if(count == 4)
                {
                    count = 0;
                    b = true;
                }

                if(buildData.isUse == false)
                {
                    removeBuildList.Add(build);
                }
            }
        }

        count = 0;
        b = false;
        // ���� removeCard ����
        foreach (List<SpecialCardData> specialDataList in specialDict.Values)
        {
            foreach (SpecialCardData specialData in specialDataList)
            {
                if (b == true)
                {
                    b = false;
                    specialScrollTrm.sizeDelta += new Vector2(0, 330f);
                }
                RemoveSpecialCard special = Instantiate(specialCardPrefab, specialScrollTrm);

                special.Init(DataManager.Instance.GetSpecialCardSO(specialData.id), specialData.isUse);
                special.button.onClick.AddListener(() =>
                {
                    int closerID = specialData.id;

                    OnClickSpecialRemove(closerID, special);
                    Debug.Log("c");
                });

                count++;
                if (count == 4)
                {
                    count = 0;
                    b = true;
                }

                if (specialData.isUse == false)
                {
                    removeSpecialList.Add(special);
                }
            }
        }
    }

    private void OnClickBuildRemove(int id, RemoveBuildCard build)
    {
        // isUseȮ���ϰ� remove �ϰų� remove ���� �ϰų�
        if(saveData.buildDataList[id].isUse == true)
        {
            if (removeCount < maxRemoveCount)
            {
                saveData.buildDataList[id].isUse = false;

                removeCount++;

                build.ActiveRemoveImage(false);

                removeBuildList.Add(build);
            }
            else 
            {
                UITooltip.Instance.Show("���� ���� Ƚ���� �����մϴ�, ���׷��̵带 �ϼ���", 1f);
            }
        }
        else
        {
            removeCount--;

            saveData.buildDataList[id].isUse = true;

            build.ActiveRemoveImage(true);

            removeBuildList.Remove(build);
        }

        ApplyUI();

        SaveFile.SaveGame();
    }
    private void OnClickSpecialRemove(int id, RemoveSpecialCard special)
    {
        // isUseȮ���ϰ� remove �ϰų� remove ���� �ϰų�
        if (saveData.speicialCardDataList[id].isUse == true)
        {
            if (removeCount < maxRemoveCount)
            {
                saveData.speicialCardDataList[id].isUse = false;

                removeCount++;

                special.ActiveRemoveImage(false);

                removeSpecialList.Add(special);
            }
            else
            {
                // tooltip
            }
        }
        else
        {
            removeCount--;

            saveData.speicialCardDataList[id].isUse = true;

            special.ActiveRemoveImage(true);

            removeSpecialList.Remove(special);
        }

        ApplyUI();

        SaveFile.SaveGame();
    }

    private void ApplyUI()
    {
        countText.text = $"{removeCount} / {maxRemoveCount}";

        goldText.text = saveData.crystal.ToString();

        foreach (RectTransform item in rightListTrm)
        {
            Destroy(item.gameObject);
        }
        rightListTrm.sizeDelta = Vector2.zero;

        foreach (RemoveBuildCard build in removeBuildList)
        {
            GameObject obj = Instantiate(rightListPrefab, rightListTrm);
            RightListUI rightListUI = obj.GetComponent<RightListUI>();
            rightListUI.Init(CardType.Build, build.buildSO.grade, build.buildSO.buildName);
            rightListUI.button.onClick.AddListener(() =>
            {
                OnClickBuildRemove(build.buildSO.id, build);
            });
            rightListTrm.sizeDelta += new Vector2(0, 100);
        }

        foreach(RemoveSpecialCard special in removeSpecialList)
        {
            GameObject obj = Instantiate(rightListPrefab, rightListTrm);
            RightListUI rightListUI = obj.GetComponent<RightListUI>();
            rightListUI.Init(CardType.Special, special.specialSO.grade, special.specialSO.specialCardName);
            rightListUI.button.onClick.AddListener(() =>
            {
                OnClickSpecialRemove(special.specialSO.id, special);
            });
            rightListTrm.sizeDelta += new Vector2(0, 100);
        }
    }

    public void UpgradeCount()
    {
        int price = 3 + saveData.maxRemoveCount * 3;
        if (saveData.crystal >= price)
        {
            saveData.crystal -= price;

            saveData.maxRemoveCount++;
            SaveFile.SaveGame();

            maxRemoveCount = saveData.maxRemoveCount;

            ApplyUI();

            UpgardeRemoveCountEvent?.Invoke();
        }
        else
        {
            UITooltip.Instance.Show("���� �����մϴ�");
        }
    }

    public int GetUpgradePrice()
    {
        return 3 + saveData.maxRemoveCount * 3;
    }
}
