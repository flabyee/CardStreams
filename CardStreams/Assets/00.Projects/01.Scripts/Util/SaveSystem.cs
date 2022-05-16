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
     * [SerializeField] private �ϳ��� �ν����Ϳ� ���� ���尡������ ����
     * [HideInInspector] �ν����Ϳ� ���� ����
     */
    private readonly static string _heroDataName = "StatList";
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
            Debug.Log($"���� �Ϸ� : {saveJson}");

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
            Debug.Log("�ε� �Ϸ� : " + loadJson);

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

    // ��ȭ ����, �ر� ����, 
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

