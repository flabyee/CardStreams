using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveFile // 세이브파일 관리하는 스크립트
{
    private static SaveData recentFile; // 최근버전 세이브(파일에 저장안한상태)

    /// <summary> 현재 세이브파일을 불러옵니다. </summary>
    public static SaveData GetSaveData()
    {
        if(recentFile == null) // 없으면 SaveSystem에서 불러오기
        {
            recentFile = SaveSystem.Load();
        }

        return recentFile;
    }

    /// <summary> 현재 세이브파일을 저장합니다. </summary>
    public static void SaveGame() // 스크립트와 같은 오브젝트에 있는 Event Listener에서 호출됩니다.
    {
        if(recentFile != null) SaveSystem.Save(recentFile);
    }

    // 평소에는 사용하면 안되는 함수다
    public static void ReloadSaveData()
    {
        recentFile = SaveSystem.Load();
    }
}
