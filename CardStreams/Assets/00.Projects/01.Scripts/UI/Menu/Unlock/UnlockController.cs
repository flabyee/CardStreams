using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockController : MonoBehaviour
{
    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    private List<BuildSO> buildList;
    private List<SpecialCardSO> specialList;

    private SaveData saveData;

    private void OnEnable()
    {
        saveData = SaveSystem.Load();

        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;
        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialList = specialListSO.specialCardList;


        for (int i = 0; i < 5; i++)
        {
            buildDict[(CardGrade)i] = new List<BuildData>();
            specialDict[(CardGrade)i] = new List<SpecialCardData>();
        }

        // 언락 안되어있는 건물과 특수카드 dict에 넣기
        foreach (BuildData itemData in saveData.buildDataList)
        {
            if (itemData.isUnlock == false)
            {
                BuildSO buildSO = buildList.Find((x) => x.id == itemData.id);

                buildDict[buildSO.grade].Add(itemData);
            }
        }

        foreach (SpecialCardData itemData in saveData.speicialCardDataList)
        {
            if (itemData.isUnlock == false)
            {
                SpecialCardSO specialSO = specialList.Find((x) => x.id == itemData.id);

                specialDict[specialSO.grade].Add(itemData);
            }
        }
    }

    public void OnClick(int cardGrade)
    {
        if(Random.Range(0, 2) == 0)
        {
            UnlockRandomBuildCard((CardGrade)cardGrade);
        }
        else
        {
            UnlockRandomSpecialCard((CardGrade)cardGrade);
        }

        SaveSystem.Save(saveData);
    }

    private void UnlockRandomBuildCard(CardGrade cardGrade)
    {
        if(buildDict[cardGrade].Count == 0)
        {
            Debug.Log("b이미 모두 해금");
            return;
        }

        int randomIndex = Random.Range(0, buildDict[cardGrade].Count);
        BuildData buildData = buildDict[cardGrade][randomIndex];

        Debug.Log("건카 해금 id : " + buildData.id);

        saveData.buildDataList[buildData.id].isUnlock = true;
        saveData.buildDataList[buildData.id].isUse = true;

        buildDict[cardGrade].Remove(buildData);
    }

    private void UnlockRandomSpecialCard(CardGrade cardGrade)
    {
        if (specialDict[cardGrade].Count == 0)
        {
            Debug.Log("s이미 모두 해금");
            return;
        }

        int randomIndex = Random.Range(0, specialDict[cardGrade].Count);
        SpecialCardData specialData = specialDict[cardGrade][randomIndex];

        Debug.Log("특카 해금 id : " + specialData.id);

        saveData.speicialCardDataList[specialData.id].isUnlock = true;
        saveData.speicialCardDataList[specialData.id].isUse = true;

        specialDict[cardGrade].Remove(specialData);
    }







    // test 코드
    public void ResetData(bool b)
    {
        foreach(BuildData buildData in saveData.buildDataList)
        {
            buildData.isUnlock = b;
            buildData.isUse = b;
        }
        foreach(SpecialCardData specialData in saveData.speicialCardDataList)
        {
            specialData.isUnlock = b;
            specialData.isUse = b;
        }

        saveData.maxRemoveCount = 5;

        SaveSystem.Save(saveData);
    }
}
