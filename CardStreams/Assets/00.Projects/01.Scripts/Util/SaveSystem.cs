using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

public static class SaveSystem
{
    private static int buildCount = 5;
    private static int specialCardCount = 9;

    /*
     * [SerializeField] private �ϳ��� �ν����Ϳ� ���� ���尡������ ����
     * [HideInInspector] �ν����Ϳ� ���� ����
     */
    private readonly static string _heroDataName = "StatList";

    private readonly static string specialStr = "0,1,2,3,4,5,6,7,8,9,12,14,27";
    private readonly static string buildStr = "0,1,2,4,5,6,8,9,10,12,13,14,16,17,18";

    // private readonly static string _haveDataName = "HaveList"; // ���߿� ��ȭ��������Ʈ �����

    private static string SaveFilePath
    {
        get { return Path.Combine(Application.persistentDataPath, $"{_heroDataName}.testJson"); }
    }

    public static void Save(SaveData saveData)
    {
        // ������ Ŭ������ Json���� �ٲ��ֱ�
        string saveJson = JsonUtility.ToJson(saveData, true);

        // Json ��ȣȭ���ֱ� : ## ������ ��
        // string encryptJson = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(saveJson));

        // using���� Stream ����(Dispose)
        using (StreamWriter sw = new StreamWriter(SaveFilePath, false, Encoding.UTF8)) // ��� | ����� | UTF-8
        {
            // ��ȣȭ�� Json�� ���Ͽ� �ۼ����ֱ� (StreamWriter ctor �ɼ����� Append/������ ��)
            sw.WriteLine(saveJson);
            //Debug.Log($"���� �Ϸ� : {saveJson}");

            // Stream �ݱ�
            sw.Close();
        }
    }

    public static SaveData Load()
    {
        // �ε�� �� Ŭ���� �ϳ� ������ֱ�
        SaveData saveData = new SaveData();

        // ��ο� ������ ������ FirstSave
        if (!File.Exists(SaveFilePath))
        {
            saveData = FirstSave();

            return saveData;
        }

        // using���� Stream ����(Dispose)
        using (StreamReader sr = new StreamReader(SaveFilePath, Encoding.UTF8)) // ��� | UTF-8
        {
            // Json �ε��ϱ�  : ## ���ڵ� ������ ��
            string loadJson = sr.ReadToEnd();
            // string decodeJson = Encoding.UTF8.GetString(System.Convert.FromBase64String(loadJson));
            //Debug.Log($"base64 ���ڵ� �Ϸ� : {loadJson}");

            // �� Ŭ������ Json�� Ŭ������ ������ #���⼭ �������������
            JsonUtility.FromJsonOverwrite(loadJson, saveData);
            //Debug.Log("�ε� �Ϸ� : " + loadJson);

            // Stream �ݱ�
            sr.Close();
        }

        buildCount = Resources.Load<BuildListSO>(typeof(BuildListSO).Name).buildList.Count;
        specialCardCount = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name).specialCardList.Count;
        // ������ �ٸ���
        if (saveData.buildDataList.Count != buildCount || saveData.speicialCardDataList.Count != specialCardCount)
        {
            return FirstSave();
        }

        // �׽�Ʈ�� Debug.Log ����
        // #TODO : �������� ������������?
        return saveData;
    }

    public static SaveData FirstSave()
    {
        SaveData saveData = new SaveData();

        saveData.crystal = 30;

        buildCount = Resources.Load<BuildListSO>(typeof(BuildListSO).Name).buildList.Count;
        specialCardCount = Resources.Load<SpecialCardListSO>(typeof(SpecialCardListSO).Name).specialCardList.Count;

        saveData.buildDataList = new List<BuildCardData>();
        saveData.speicialCardDataList = new List<SpecialCardData>();

        List<string> unlockList = buildStr.Split(',').ToList();
        List<int> unlockIntList = new List<int>();
        foreach (var item in unlockList)
            unlockIntList.Add(int.Parse(item));

        for (int i = 0; i < buildCount; i++)
        {
            if(unlockIntList.Contains(i))
                saveData.buildDataList.Add(new BuildCardData() { id = i, isUnlock = true, isUse = true });
            else
                saveData.buildDataList.Add(new BuildCardData() { id = i, isUnlock = false, isUse = false });
        }

        unlockList = specialStr.Split(',').ToList();
        unlockIntList = new List<int>();
        foreach (var item in unlockList)
            unlockIntList.Add(int.Parse(item));

        for (int i = 0; i < specialCardCount; i++)
        {
            if(unlockIntList.Contains(i))
                saveData.speicialCardDataList.Add(new SpecialCardData() { id = i, isUnlock = true, isUse = true });
            else
                saveData.speicialCardDataList.Add(new SpecialCardData() { id = i, isUnlock = false, isUse = false });
        }

        // ������ Ŭ������ Json���� �ٲ��ֱ�
        string saveJson = JsonUtility.ToJson(saveData, true);

        // Json ��ȣȭ���ֱ� : ## ������ ��
        // string encryptJson = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(saveJson));

        // using���� Stream ����(Dispose)
        using (StreamWriter sw = new StreamWriter(SaveFilePath, false, Encoding.UTF8)) // ��� | ����� | UTF-8
        {
            // Json�� ���Ͽ� �ۼ����ֱ� (StreamWriter ctor �ɼ����� Append/������ ��)
            sw.WriteLine(saveJson);
            Debug.Log($"ù ���� �Ϸ� : {saveJson}");

            // Stream �ݱ�
            sw.Close();
        }

        return saveData;
    }
}

[System.Serializable]
public class SaveData
{
    // ���� ���� ( ��ȭ, ������ �ü�, Ŭ������ �������� ����?? <- �̰Ŵ� ����? ���)
    public int crystal;
    // ��ȭ ����, �ر� ����, 
    public List<SpecialCardData> speicialCardDataList;
    public List<BuildCardData> buildDataList;

    public int maxRemoveCount;
    public int prestige; // ��ġ
}

[System.Serializable]
public class SpecialCardData
{
    public int id;
    public bool isUnlock;
    public bool isUse;
}

[System.Serializable]
public class BuildCardData
{
    public int id;
    public bool isUnlock;
    public bool isUse;
}

