using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockController : MonoBehaviour
{
    private Dictionary<CardGrade, List<BuildData>> buildDict = new Dictionary<CardGrade, List<BuildData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    private List<BuildSO> buildList;
    private List<SpecialCardSO> specialList;

    private SaveData saveData;

    public TextMeshProUGUI goldText;

    public void OnOpen()
    {
        saveData = SaveSystem.Load();

        ApplyUI();

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
     
    public void OnClickBuildUnlock(int cardGrade)
    {
        if(saveData.gold >= 5)
        {
            saveData.gold -= 5;

            bool b = UnlockRandomBuildCard((CardGrade)cardGrade);

            if (b == false)
            {
                saveData.gold += 5;
                UITooltip.Instance.Show("이미 해당 등급의 건물카드를 해금하였습니다", new UITooltip.TooltipTimer(2f));
            }

            SaveSystem.Save(saveData);
            ApplyUI();
        }
        else
        {
            UITooltip.Instance.Show("돈이 부족합니다", new UITooltip.TooltipTimer(2f));
        }
    }

    public void OnClickSpecialUnlock(int cardGrade)
    {
        if (saveData.gold >= 5)
        {
            saveData.gold -= 5;

            bool b = UnlockRandomSpecialCard((CardGrade)cardGrade);

            if (b == false)
            {
                saveData.gold += 5;
                UITooltip.Instance.Show("이미 해당 등급의 특수카드를 해금하였습니다", new UITooltip.TooltipTimer(2f));
            }

            SaveSystem.Save(saveData);
            ApplyUI();
        }
        else
        {
            UITooltip.Instance.Show("돈이 부족합니다", new UITooltip.TooltipTimer(2f));
        }
    }

    private bool UnlockRandomBuildCard(CardGrade cardGrade)
    {
        if(buildDict[cardGrade].Count == 0)
        {
            Debug.Log("b이미 모두 해금");
            return false;
        }

        int randomIndex = Random.Range(0, buildDict[cardGrade].Count);
        BuildData buildData = buildDict[cardGrade][randomIndex];

        Debug.Log("건카 해금 id : " + buildData.id);

        saveData.buildDataList[buildData.id].isUnlock = true;
        saveData.buildDataList[buildData.id].isUse = true;

        buildDict[cardGrade].Remove(buildData);

        return true;


    }

    private bool UnlockRandomSpecialCard(CardGrade cardGrade)
    {
        if (specialDict[cardGrade].Count == 0)
        {
            Debug.Log("s이미 모두 해금");
            return false;
        }

        int randomIndex = Random.Range(0, specialDict[cardGrade].Count);
        SpecialCardData specialData = specialDict[cardGrade][randomIndex];

        Debug.Log("특카 해금 id : " + specialData.id);

        saveData.speicialCardDataList[specialData.id].isUnlock = true;
        saveData.speicialCardDataList[specialData.id].isUse = true;

        specialDict[cardGrade].Remove(specialData);

        return true;
    }

    private void ApplyUI()
    {
        goldText.text = saveData.gold.ToString();
    }

    public void OnClose()
    {

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

        SaveSystem.Save(saveData);
    }

    public void OnClickDoGam()
    {

    }
}
