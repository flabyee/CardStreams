using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : MonoBehaviour // 세이브파일 관리하는 스크립트
{
    public static SaveData recentFile; // 최근버전 세이브(저장안함)

    private void Awake()
    {
        recentFile = SaveSystem.Load();
    }

    /// <summary> 현재 세이브파일을 저장합니다. </summary>
    public void SaveCurrentFile() // 스크립트와 같은 오브젝트에 있는 Event Listener에서 호출됩니다.
    {
        SaveSystem.Save(recentFile);
    }
}
