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

    // ������ ����Ÿ
    private Dictionary<int, int> haveBuildDic = new Dictionary<int, int>();
    private Dictionary<int, int> haveSpecialDic = new Dictionary<int, int>();


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
        specialCardList = specialListSO.specialCardListSO;

        StageDataListSO stageDataListSO = Resources.Load<StageDataListSO>(typeof(StageDataListSO).Name);
        stageDataList = stageDataListSO.stageDataList;


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
    public Dictionary<int, int> GetHaveSpecialCardDic()
    {
        return haveSpecialDic;
    }
    public void AddSpecialCard(int itemID, int amount = 1)
    {
        // ���������� ���� ������ ���Ŵ� Add
        if (haveSpecialDic.ContainsKey(itemID) == false)
        {
            haveSpecialDic.Add(itemID, amount);
        }
        // �������� ������ ���Ŵ� +=
        else
        {
            haveSpecialDic[itemID] += amount;
        }
        
        //Save(false, false, true);
    }
    public bool IsHaveSpecialCard(int itemID)
    {
        return haveSpecialDic.ContainsKey(itemID);
    }
    public void RemoveSpecialCard(int itemID, int amount = 1)
    {
        if (haveSpecialDic.ContainsKey(itemID))
        {
            if (haveSpecialDic[itemID] >= amount)
            {
                haveSpecialDic[itemID] -= amount;

                // �����ִ� �������� ������ haveItemDic���� ����
                if (haveSpecialDic[itemID] == 0)
                {
                    haveSpecialDic.Remove(itemID);
                }

                //Save(false, false, true);
            }
            else
            {
                Debug.LogError("������� ������ �������� �������� ����");
            }
        }
        else
        {
            Debug.LogError("�������� ���� �������� ����� ��");
        }
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
    public Dictionary<int, int> GetHaveBuildDic()
    {
        return haveBuildDic;
    }
    public void AddBuild(int itemID, int amount = 1)
    {
        // ���������� ���� ������ ���Ŵ� Add
        if (haveBuildDic.ContainsKey(itemID) == false)
        {
            haveBuildDic.Add(itemID, amount);
        }
        // �������� ������ ���Ŵ� +=
        else
        {
            haveBuildDic[itemID] += amount;
        }

        //Save(false, false, true);
    }
    public bool IsHaveBuild(int itemID)
    {
        return haveBuildDic.ContainsKey(itemID);
    }
    public void RemoveBuild(int itemID, int amount = 1)
    {
        if (haveBuildDic.ContainsKey(itemID))
        {
            if (haveBuildDic[itemID] >= amount)
            {
                haveBuildDic[itemID] -= amount;

                // �����ִ� �������� ������ haveItemDic���� ����
                if (haveBuildDic[itemID] == 0)
                {
                    haveBuildDic.Remove(itemID);
                }

                //Save(false, false, true);
            }
            else
            {
                Debug.LogError("������� ������ �������� �������� ����");
            }
        }
        else
        {
            Debug.LogError("�������� ���� �������� ����� ��");
        }
    }






    // stage ����
    public StageDataSO GetNowStageData()
    {
        return stageDataList[stageNumValue.RuntimeValue];
    }
}