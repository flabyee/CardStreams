using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipController : MonoBehaviour
{
    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    private List<BuildSO> buildList;
    private List<SpecialCardSO> specialList;

    private SaveData saveData;



    private int maxBuildRemoveCount;
    private int buildRemoveCount;
    private int maxSpecialRemoveCount;
    private int specialRemoveCount;

    public RectTransform buildScrollTrm;
    public RectTransform specialScrollTrm;

    public RemoveBuildCard buildCardPrefab;
    public RemoveSpecialCard specialCardPrefab;

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

        maxBuildRemoveCount = saveData.maxRemoveCount;
        maxSpecialRemoveCount = saveData.maxRemoveCount;

        buildRemoveCount = 0;
        specialRemoveCount = 0;

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
        buildScrollTrm.sizeDelta = new Vector2(0, 300);
        specialScrollTrm.sizeDelta = new Vector2(0, 300);

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
                    buildRemoveCount++;
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
                    specialRemoveCount++;
                }
            }
        }

        CreateCard();
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
                    buildScrollTrm.sizeDelta += new Vector2(0, 300);
                }
                RemoveBuildCard build = Instantiate(buildCardPrefab, buildScrollTrm);

                build.Init(DataManager.Instance.GetBuildSO(buildData.id), buildData.isUse);
                build.button.onClick.AddListener(() =>
                {
                    int closerID = buildData.id;

                    OnClickBuildRemove(closerID, build);
                });

                count++;
                if(count == 6)
                {
                    count = 0;
                    b = true;
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
                    specialScrollTrm.sizeDelta += new Vector2(0, 300);
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
                if (count == 6)
                {
                    count = 0;
                    b = true;
                }
            }
        }
    }

    private void OnClickBuildRemove(int id, RemoveBuildCard build)
    {
        // isUse확인하고 remove 하거나 remove 해제 하거나
        if(saveData.buildDataList[id].isUse == true)
        {
            if (buildRemoveCount < maxBuildRemoveCount)
            {
                saveData.buildDataList[id].isUse = false;

                buildRemoveCount++;

                build.ActiveRemoveImage(false);
            }
            else 
            {
                // tooltip
            }
        }
        else
        {
            buildRemoveCount--;

            saveData.buildDataList[id].isUse = true;

            build.ActiveRemoveImage(true);
        }

        SaveSystem.Save(saveData);
    }
    private void OnClickSpecialRemove(int id, RemoveSpecialCard special)
    {
        // isUse확인하고 remove 하거나 remove 해제 하거나
        if (saveData.speicialCardDataList[id].isUse == true)
        {
            if (specialRemoveCount < maxSpecialRemoveCount)
            {
                saveData.speicialCardDataList[id].isUse = false;

                specialRemoveCount++;

                special.ActiveRemoveImage(false);
            }
            else
            {
                // tooltip
            }
        }
        else
        {
            specialRemoveCount--;

            saveData.speicialCardDataList[id].isUse = true;

            special.ActiveRemoveImage(true);
        }

        SaveSystem.Save(saveData);
    }
}
