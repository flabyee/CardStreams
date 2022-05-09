using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    List<BuildSO> buildList;
    List<SpecialCardSO> specialCardList;

    // 저장할 데이타
    private Dictionary<int, int> haveBuildDic = new Dictionary<int, int>();
    private Dictionary<int, int> haveSpecialDic = new Dictionary<int, int>();


    // 엑셀로 부터 가져올 정보
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


    // special card 관련
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
        // 보유중이지 않은 아이템 구매는 Add
        if (haveSpecialDic.ContainsKey(itemID) == false)
        {
            haveSpecialDic.Add(itemID, amount);
        }
        // 보유중인 아이템 구매는 +=
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

                // 남아있는 아이템이 없으면 haveItemDic에서 제거
                if (haveSpecialDic[itemID] == 0)
                {
                    haveSpecialDic.Remove(itemID);
                }

                //Save(false, false, true);
            }
            else
            {
                Debug.LogError("지우려는 갯수가 보유중인 갯수보다 많음");
            }
        }
        else
        {
            Debug.LogError("보유하지 않은 아이템을 지우려 함");
        }
    }


    // build 관련
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
        // 보유중이지 않은 아이템 구매는 Add
        if (haveBuildDic.ContainsKey(itemID) == false)
        {
            haveBuildDic.Add(itemID, amount);
        }
        // 보유중인 아이템 구매는 +=
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

                // 남아있는 아이템이 없으면 haveItemDic에서 제거
                if (haveBuildDic[itemID] == 0)
                {
                    haveBuildDic.Remove(itemID);
                }

                //Save(false, false, true);
            }
            else
            {
                Debug.LogError("지우려는 갯수가 보유중인 갯수보다 많음");
            }
        }
        else
        {
            Debug.LogError("보유하지 않은 아이템을 지우려 함");
        }
    }






    // stage 관련
    public void GetStageData(int index)
    {

    }
}


// 각 스테이지에 대한 정보를 가지고 있는 클래스
// 정보 : 맵, 덱, 난이도?를 가지고 있는다
public class StageData
{
    public string mapStr;
    public List<CardData> deck;
    
    // 나중에 난이도 추가
}