using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ���� �ڿ������� �ڿ���Ȳ ������Ʈ�Ҷ� �̰� �����, ������Ʈ Event ����ξ�����
// enum�� add use�� �߰��ؾ��ҵ�?

public enum ResourceType
{
    crystal,
    prestige
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public UnityEvent OnResourceChanged; // ��ġ �ٲ����� ���� ��ɵ� �־��ּ���

    [HideInInspector] public int crystal;
    [HideInInspector] public int prestige;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SaveData saveData = SaveFile.GetSaveData();
        crystal = saveData.crystal;
        prestige = saveData.prestige;
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.crystal:
                crystal += amount;
                break;
            case ResourceType.prestige:
                prestige += amount;
                break;
        }
        OnResourceChanged?.Invoke();
        
    }

    /// <summary> �ڿ��� ����ϸ� ����ϴ� ��� </summary>
    /// <param name="type">�ڿ��� Ÿ��</param>
    /// <param name="amount">�󸶳� ����</param>
    /// <returns>����ϸ� true(���), �����ϸ� false(�̻��)</returns>
    public bool UseResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.crystal:
                if (crystal < amount) return false;
                crystal -= amount;
                break;
            case ResourceType.prestige:
                if (prestige < amount) return false;
                prestige -= amount;
                break;
        }
        OnResourceChanged?.Invoke();
        return true;
    }

    public void SendSaveFile() // ���̺����� �ҷ��ͼ� Resource �κи� �������ִ� �Լ�
    {
        SaveData saveData = SaveFile.GetSaveData();
        saveData.crystal = crystal;
        saveData.prestige = prestige;
    }
}
