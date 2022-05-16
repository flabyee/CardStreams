using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public static class SaveSystem
{
    private static int buildCount = 5;
    private static int specialCardCount = 9;

    /*
     * [SerializeField] private 하나를 인스펙터에 띄우고 저장가능으로 만듬
     * [HideInInspector] 인스펙터에 띄운걸 지움
     */
    private readonly static string _heroDataName = "StatList";
    // private readonly static string _haveDataName = "HaveList"; // 나중에 재화보관리스트 만들기

    private static string SaveFilePath
    {
        get { return Path.Combine(Application.persistentDataPath, $"{_heroDataName}.testJson"); }
    }

    public static void Save(SaveData saveData)
    {
        // 저장할 클래스를 Json으로 바꿔주기
        string saveJson = JsonUtility.ToJson(saveData, true);

        // Json 암호화해주기 : ## 지금은 뺌
        // string encryptJson = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(saveJson));

        // using으로 Stream 조절(Dispose)
        using (StreamWriter sw = new StreamWriter(SaveFilePath, false, Encoding.UTF8)) // 경로 | 덮어쓰기 | UTF-8
        {
            // 암호화된 Json을 파일에 작성해주기 (StreamWriter ctor 옵션으로 Append/덮어씌우기 됨)
            sw.WriteLine(saveJson);
            Debug.Log($"저장 완료 : {saveJson}");

            // Stream 닫기
            sw.Close();
        }
    }

    public static SaveData Load()
    {
        // 로드용 빈 클래스 하나 만들어주기
        SaveData saveData = new SaveData();

        // 경로에 파일이 없으면 FirstSave
        if (!File.Exists(SaveFilePath))
        {
            saveData = FirstSave();

            return saveData;
        }

        // using으로 Stream 조절(Dispose)
        using (StreamReader sr = new StreamReader(SaveFilePath, Encoding.UTF8)) // 경로 | UTF-8
        {
            // Json 로드하기  : ## 디코딩 지금은 뺌
            string loadJson = sr.ReadToEnd();
            // string decodeJson = Encoding.UTF8.GetString(System.Convert.FromBase64String(loadJson));
            //Debug.Log($"base64 디코딩 완료 : {loadJson}");

            // 빈 클래스에 Json을 클래스로 덮어씌우기 #여기서 문제생길수있음
            JsonUtility.FromJsonOverwrite(loadJson, saveData);
            Debug.Log("로드 완료 : " + loadJson);

            // Stream 닫기
            sr.Close();
        }

        buildCount = Resources.Load<BuildListSO>(typeof(BuildListSO).Name).buildList.Count;
        specialCardCount = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name).specialCardList.Count;
        // 갯수가 다르면
        if (saveData.buildDataList.Count != buildCount || saveData.speicialCardDataList.Count != specialCardCount)
        {
            return FirstSave();
        }

        // 테스트용 Debug.Log 띄우기
        // #TODO : 아직없음 만들어야할지도?
        return saveData;
    }

    public static SaveData FirstSave()
    {
        SaveData saveData = new SaveData();

        buildCount = Resources.Load<BuildListSO>(typeof(BuildListSO).Name).buildList.Count;
        specialCardCount = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name).specialCardList.Count;

        saveData.buildDataList = new List<BuildData>();
        saveData.speicialCardDataList = new List<SpecialCardData>();
        for (int i = 0; i < buildCount; i++)
        {
            saveData.buildDataList.Add(
                new BuildData() { id = i, haveAmount = 0, isUnlock = true, upgradeAmount = 0 });
        }
        for(int i = 0; i < specialCardCount; i++)
        {
            saveData.speicialCardDataList.Add(
                new SpecialCardData() { id = i, haveAmount = 0, isUnlock = true, upgradeAmount = 0 });
        }

        // 저장할 클래스를 Json으로 바꿔주기
        string saveJson = JsonUtility.ToJson(saveData, true);

        // Json 암호화해주기 : ## 지금은 뺌
        // string encryptJson = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(saveJson));

        // using으로 Stream 조절(Dispose)
        using (StreamWriter sw = new StreamWriter(SaveFilePath, false, Encoding.UTF8)) // 경로 | 덮어쓰기 | UTF-8
        {
            // Json을 파일에 작성해주기 (StreamWriter ctor 옵션으로 Append/덮어씌우기 됨)
            sw.WriteLine(saveJson);
            Debug.Log($"첫 저장 완료 : {saveJson}");

            // Stream 닫기
            sw.Close();
        }

        return saveData;
    }
}

[System.Serializable]
public class SaveData
{
    // 마을 관련 ( 재화, 마을의 시설, 클리어한 스테이지 정보?? <- 이거는 따로? 등등)

    // 강화 상태, 해금 상태, 
    public List<SpecialCardData> speicialCardDataList;
    public List<BuildData> buildDataList;
}

[System.Serializable]
public class SpecialCardData
{
    public int id;
    public bool isUnlock;
    public int upgradeAmount;
    public int haveAmount;
}

[System.Serializable]
public class BuildData
{
    public int id;
    public bool isUnlock;
    public int upgradeAmount;
    public int haveAmount;
}

