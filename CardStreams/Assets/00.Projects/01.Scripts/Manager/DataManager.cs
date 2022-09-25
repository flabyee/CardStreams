using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private List<BuildSO> buildCardList;
    private List<SpecialCardSO> specialCardList;

    // buildCardList와 specialCardList를 등급별로 분리해놓은 Dict
    private Dictionary<CardGrade, List<BuildCardData>> buildDict = new Dictionary<CardGrade, List<BuildCardData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    private List<StageDataSO> stageDataList;



    public IntValue stageNumValue;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        #region 건물, 특수 카드 SO 불러오고 등급별로 분리
        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        this.buildCardList = buildListSO.buildList;

        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialCardList = specialListSO.specialCardList;

        for (int i = 0; i < 5; i++)
        {
            buildDict[(CardGrade)i] = new List<BuildCardData>();
            specialDict[(CardGrade)i] = new List<SpecialCardData>();
        }

        // 언락 되어있는 건물과 특수카드 dict에 넣기
        foreach (BuildCardData itemData in SaveFile.GetSaveData().buildDataList)
        {
            if (itemData.isUnlock == true && itemData.isUse == true)
            {
                BuildSO buildSO = this.buildCardList.Find((x) => x.id == itemData.id);

                buildDict[buildSO.grade].Add(itemData);
            }
        }

        foreach (SpecialCardData itemData in SaveFile.GetSaveData().speicialCardDataList)
        {
            if (itemData.isUnlock == true && itemData.isUse == true)
            {
                SpecialCardSO specialSO = specialCardList.Find((x) => x.id == itemData.id);
                specialDict[specialSO.grade].Add(itemData);
            }
        }
        #endregion

        StageDataListSO stageDataListSO = Resources.Load<StageDataListSO>(typeof(StageDataListSO).Name);
        stageDataList = stageDataListSO.stageDataList;
    }


    // special card 관련
    public List<SpecialCardData> GetSpecialDataList(CardGrade grade)
    {
        return specialDict[grade];
    }
    public SpecialCardSO GetSpecialCardSO(int index)
    {
        return specialCardList.Find((x) => x.id == index);
    }


    // build 관련
    public List<BuildCardData> GetBuildDataList(CardGrade grade)
    {
        return buildDict[grade];
    }
    public BuildSO GetBuildSO(int id)
    {
        BuildSO buildSO = buildCardList.Find((x) => x.id == id);
        if(buildSO == null)
        {
            Debug.LogError($"{id}buildSO is null");
        }
        return buildSO;
    }
    public BuildSO GetBuildSO(CardGrade grade, int index)
    {
        BuildCardData buildData = buildDict[grade][index];

        BuildSO buildSO = buildCardList.Find((x) => x.id == buildData.id);
        if (buildSO == null)
        {
            Debug.LogError($"{buildData.id}buildSO is null");
        }
        return buildSO;
    }
    public BuildSO GetRandomBuildSO()
    {
        return buildCardList[UnityEngine.Random.Range(0, buildCardList.Count)];
    }
    public BuildSO GetRandomBuildSO(CardGrade grade)
    {
        BuildCardData buildData = buildDict[grade][UnityEngine.Random.Range(0, buildDict[grade].Count)];
        return GetBuildSO(buildData.id);
    }





    // stage 관련
    public StageDataSO GetNowStageData()
    {
        return stageDataList[stageNumValue.RuntimeValue];
    }
    public StageDataSO GetStageData(int stageNum)
    {
        return stageDataList[stageNum];
    }
    public SpecialCardSO GetSpecialCardSO(CardGrade grade, int index)
    {
        SpecialCardData specialData = specialDict[grade][index];

        SpecialCardSO specialSO = specialCardList.Find((x) => x.id == specialData.id);
        if (specialSO == null)
        {
            Debug.LogError($"{specialData.id}specialSO is null");
        }
        return specialSO;
    }
    public SpecialCardSO GetRandomSpecialSO()
    {
        return specialCardList[UnityEngine.Random.Range(0, specialCardList.Count)];
    }
    public SpecialCardSO GetRandomSpecialSO(CardGrade grade)
    {
        SpecialCardData specialData = specialDict[grade][UnityEngine.Random.Range(0, specialDict[grade].Count)];
        return GetSpecialCardSO(specialData.id);
    }
}