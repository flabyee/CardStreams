using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockController : MonoBehaviour
{
    private Dictionary<CardGrade, List<BuildCardData>> buildDict = new Dictionary<CardGrade, List<BuildCardData>>();
    private Dictionary<CardGrade, List<SpecialCardData>> specialDict = new Dictionary<CardGrade, List<SpecialCardData>>();

    private List<BuildSO> buildList;
    private List<SpecialCardSO> specialList;

    private SaveData saveData;

    public TextMeshProUGUI goldText;

    [Header("GetPanel")]
    public GameObject getPanelObj;

    public GameObject buildObj;
    public TextMeshProUGUI build_nameText;
    public TextMeshProUGUI build_infoText;
    public Image build_cardImage;

    public GameObject specialObj;
    public TextMeshProUGUI special_nameText;
    public TextMeshProUGUI special_infoText;
    public Image special_cardImage;

    public void OnOpen()
    {
        saveData = SaveFile.GetSaveData();

        ApplyUI();

        BuildListSO buildListSO = Resources.Load<BuildListSO>(typeof(BuildListSO).Name);
        buildList = buildListSO.buildList;
        SpecialCardListSO specialListSO = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name);
        specialList = specialListSO.specialCardList;

        if(specialList == null)
        {
            
        }

       

        for (int i = 0; i < 5; i++)
        {
            buildDict[(CardGrade)i] = new List<BuildCardData>();
            specialDict[(CardGrade)i] = new List<SpecialCardData>();
        }

        // 언락 안되어있는 건물과 특수카드 dict에 넣기
        foreach (BuildCardData itemData in saveData.buildDataList)
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
        int price = 3 + cardGrade * 3;
        if(saveData.crystal >= price)
        {
            saveData.crystal -= price;

            bool b = UnlockRandomBuildCard((CardGrade)cardGrade);

            if (b == false)
            {
                saveData.crystal += price;
                UITooltip.Instance.Show("이미 해당 등급의 건물카드를 모두 해금하였습니다", 0.5f);
            }

            SaveFile.SaveGame();
            ApplyUI();
        }
        else
        {
            UITooltip.Instance.Show("돈이 부족합니다", 0.5f);
        }
    }

    public void OnClickSpecialUnlock(int cardGrade)
    {
        int price = 3 + cardGrade * 3;
        if (saveData.crystal >= price)
        {
            saveData.crystal -= price;

            bool b = UnlockRandomSpecialCard((CardGrade)cardGrade);

            if (b == false)
            {
                saveData.crystal += price;
                UITooltip.Instance.Show("이미 해당 등급의 특수카드를 모두 해금하였습니다", 0.5f);
            }

            SaveFile.SaveGame();
            ApplyUI();
        }
        else
        {
            UITooltip.Instance.Show("돈이 부족합니다", 0.5f);
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
        BuildCardData buildData = buildDict[cardGrade][randomIndex];

        Debug.Log("건카 해금 id : " + buildData.id);

        saveData.buildDataList[buildData.id].isUnlock = true;
        saveData.buildDataList[buildData.id].isUse = true;

        buildDict[cardGrade].Remove(buildData);

        getPanelObj.SetActive(true);
        buildObj.SetActive(true);
        specialObj.SetActive(false);
        BuildSO buildSO = DataManager.Instance.GetBuildSO(buildData.id);
        build_nameText.text = buildSO.buildName;
        build_infoText.text = buildSO.tooltip;
        build_cardImage.sprite = buildSO.sprite;

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

        getPanelObj.SetActive(true);
        specialObj.SetActive(true);
        buildObj.SetActive(false);
        SpecialCardSO specialSO = DataManager.Instance.GetSpecialCardSO(specialData.id);
        special_nameText.text = specialSO.specialCardName;
        special_infoText.text = specialSO.tooltip;
        special_cardImage.sprite = specialSO.sprite;

        return true;
    }

    private void ApplyUI()
    {
        goldText.text = saveData.crystal.ToString();
    }

    public void OnClose()
    {

    }

    // test 코드
    public void ResetData(bool b)
    {
        foreach(BuildCardData buildData in saveData.buildDataList)
        {
            buildData.isUnlock = b;
            buildData.isUse = b;
        }
        foreach(SpecialCardData specialData in saveData.speicialCardDataList)
        {
            specialData.isUnlock = b;
            specialData.isUse = b;
        }

        SaveFile.SaveGame();
    }

    public void CloseGetPanel()
    {
        getPanelObj.SetActive(false);
    }
}
