using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public SaveData saveData;

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

        Load();
    }


    // special card ����
    public List<SpecialCardSO> GetSpecialSOList()
    {
        return specialCardList;
    }
    public SpecialCardSO GetSpecialCardSO(int index)
    {
        return specialCardList.Find((x) => x.id == index);
    }


    // build ����
    public List<BuildSO> GetBuildSOList()
    {
        return buildList;
    }
    public BuildSO GetBuildSO(int index)
    {
        return buildList.Find((x) => x.id == index);
    }







    // stage ����
    public StageDataSO GetNowStageData()
    {
        return stageDataList[stageNumValue.RuntimeValue];
    }






    // �ӽ� �ڵ�
    public void Save()
    {
        SaveSystem.Save(saveData);
    }
    public void Load()
    {
        saveData = SaveSystem.Load();
    }
}