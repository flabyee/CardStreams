using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : MonoBehaviour // ���̺����� �����ϴ� ��ũ��Ʈ
{
    public static SaveData recentFile; // �ֱٹ��� ���̺�(�������)

    private void Awake()
    {
        recentFile = SaveSystem.Load();
    }

    /// <summary> ���� ���̺������� �����մϴ�. </summary>
    public void SaveCurrentFile() // ��ũ��Ʈ�� ���� ������Ʈ�� �ִ� Event Listener���� ȣ��˴ϴ�.
    {
        SaveSystem.Save(recentFile);
    }
}
