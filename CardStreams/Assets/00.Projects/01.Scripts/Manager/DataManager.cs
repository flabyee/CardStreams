using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    List<BuildSO> buildList;
    List<SpecialCardSO> specialCardList;

    List<StageDataSO> stageDataList;



    public IntValue stageNumValue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;

        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialCardList = specialListSO.specialCardList;

        StageDataListSO stageDataListSO = Resources.Load<StageDataListSO>(typeof(StageDataListSO).Name);
        stageDataList = stageDataListSO.stageDataList;
    }


    // special card 관련
    public List<SpecialCardSO> GetSpecialSOList()
    {
        return specialCardList;
    }
    public SpecialCardSO GetSpecialCardSO(int index)
    {
        return specialCardList.Find((x) => x.id == index);
    }


    // build 관련
    public List<BuildSO> GetBuildSOList()
    {
        return buildList;
    }
    public BuildSO GetBuildSO(int index)
    {
        BuildSO buildSO = buildList.Find((x) => x.id == index);
        if(buildSO == null)
        {
            Debug.LogError($"{index}buildSO is null");
        }
        return buildSO;
    }







    // stage 관련
    public StageDataSO GetNowStageData()
    {
        return stageDataList[stageNumValue.RuntimeValue];
    }
}