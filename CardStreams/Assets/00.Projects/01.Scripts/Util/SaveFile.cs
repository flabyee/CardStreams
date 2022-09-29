using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveFile // ���̺����� �����ϴ� ��ũ��Ʈ
{
    private static SaveData recentFile; // �ֱٹ��� ���̺�(���Ͽ� ������ѻ���)

    /// <summary> ���� ���̺������� �ҷ��ɴϴ�. </summary>
    public static SaveData GetSaveData()
    {
        if(recentFile == null) // ������ SaveSystem���� �ҷ�����
        {
            recentFile = SaveSystem.Load();
        }

        return recentFile;
    }

    /// <summary> ���� ���̺������� �����մϴ�. </summary>
    public static void SaveGame() // ��ũ��Ʈ�� ���� ������Ʈ�� �ִ� Event Listener���� ȣ��˴ϴ�.
    {
        if(recentFile != null) SaveSystem.Save(recentFile);
    }

    // ��ҿ��� ����ϸ� �ȵǴ� �Լ���
    public static void ReloadSaveData()
    {
        recentFile = SaveSystem.Load();
    }
}
