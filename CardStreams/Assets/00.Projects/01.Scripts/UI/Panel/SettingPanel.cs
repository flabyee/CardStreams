using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : Panel
{
    public void OnClickExit()
    {
        ResourceManager.Instance.SendSaveFile();
        SaveFile.SaveGame();
        Application.Quit();
    }

    public void OnClickTitle()
    {
        ResourceManager.Instance.SendSaveFile();
        SaveFile.SaveGame();
        DropArea.dropAreas.Clear();
        DontRaycastTarget.dontRaycastTargetList.Clear();

        LoadingSceneManager.LoadScene("MenuScene");
    }
}
