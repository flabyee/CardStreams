using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 이제 자원쓸때나 자원현황 업데이트할때 이거 쓰면됨, 업데이트 Event 제대로쓰려면
// enum에 add use도 추가해야할듯?

public enum ResourceType
{
    crystal,
    prestige
}

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public UnityEvent OnResourceChanged; // 수치 바꿨을때 쓰는 기능들 넣어주세용

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

    /// <summary> 자원이 충분하면 사용하는 기능 </summary>
    /// <param name="type">자원의 타입</param>
    /// <param name="amount">얼마나 쓸지</param>
    /// <returns>충분하면 true(사용), 부족하면 false(미사용)</returns>
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

    public void SendSaveFile() // 세이브파일 불러와서 Resource 부분만 수정해주는 함수
    {
        SaveData saveData = SaveFile.GetSaveData();
        saveData.crystal = crystal;
        saveData.prestige = prestige;
    }
}
