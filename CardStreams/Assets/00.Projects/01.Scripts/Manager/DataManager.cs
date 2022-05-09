using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    List<BuildSO> buildList;
    List<SpecialCardSO> specialCardList;

    // ������ ����Ÿ
    private Dictionary<int, int> haveBuildDic = new Dictionary<int, int>();
    private Dictionary<int, int> haveSpecialDic = new Dictionary<int, int>();


    // ������ ���� ������ ����
    private List<StageData> stageDataList = new List<StageData>();

    private void Awake()
    {
        Instance = this;

        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;

        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialCardList = specialListSO.specialCardListSO;

        stageDataList.Add(new StageData()
        {
            mapStr = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,0,0,8,0,4,0,0,0,0,0,0,0,7,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0",
            deck = new List<CardData>()
            {
                new CardData(CardType.Sword, 3, DropAreaType.feild)
            },
        }); 
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
    public void GetStageData(int index)
    {

    }
}


// �� ���������� ���� ������ ������ �ִ� Ŭ����
// ���� : ��, ��, ���̵�?�� ������ �ִ´�
public class StageData
{
    public string mapStr;
    public List<CardData> deck;
    
    // ���߿� ���̵� �߰�
}