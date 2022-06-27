using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipController : MonoBehaviour
{
    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    // 현제 리무브 중인 리스트
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

    private void Awake()
    {
        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;
        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialList = specialListSO.specialCardList;
    }

    public void OnEquip()
    {
        saveData = SaveSystem.Load();

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
        
        buildScrollTrm.sizeDelta = new Vector2(0, 305f);
        specialScrollTrm.sizeDelta = new Vector2(0, 305f);

        for (int i = 0; i < 5; i++)
        {
            buildDict[(CardGrade)i] = new List<BuildData>();
            specialDict[(CardGrade)i] = new List<SpecialCardData>();
        }

        // 언락 되어있는 건물과 특수카드 dict에 넣기
        foreach (BuildData itemData in saveData.buildDataList)
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
            Debug.LogError("최대 제외 갯수 보다 제외한 갯수가 더 많습니다");
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
        // 건물 removeCard 생성
        foreach(List<BuildData> buildDataList in buildDict.Values)
        {
            foreach(BuildData buildData in buildDataList)
            {
                if(b == true)
                {
                    b = false;
                    buildScrollTrm.sizeDelta += new Vector2(0, 330f);
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
        // 툭수 removeCard 생성
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
        // isUse확인하고 remove 하거나 remove 해제 하거나
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
                // tooltip
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

        SaveSystem.Save(saveData);
    }
    private void OnClickSpecialRemove(int id, RemoveSpecialCard special)
    {
        // isUse확인하고 remove 하거나 remove 해제 하거나
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

        SaveSystem.Save(saveData);
    }

    private void ApplyUI()
    {
        countText.text = $"{removeCount} / {maxRemoveCount}";

        goldText.text = saveData.gold.ToString();

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
        if(saveData.gold >= 5)
        {
            saveData.gold -= 5;

            saveData.maxRemoveCount++;
            SaveSystem.Save(saveData);

            maxRemoveCount = saveData.maxRemoveCount;

            ApplyUI();
        }
        else
        {
            UITooltip.Instance.Show("돈이 부족합니다");
        }
    }
}
