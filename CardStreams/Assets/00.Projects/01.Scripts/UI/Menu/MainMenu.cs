using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu<MainMenu>
{
    public IntValue stageValue;

    public GameObject levelSelectPanel;
    public GameObject cheetPanel;

    public void OnClickPlay(int i)
    {
        stageValue.RuntimeValue = i;
        LoadingSceneManager.LoadScene("SampleScene");
    }

    public void OnClickVillage()
    {
        LoadingSceneManager.LoadScene("Village");
    }

    public void OnClickOption()
    {
        
    }

    public void OnClickEquip()
    {
        EquipMenu.Open();
    }

    public void OnClickUnlock()
    {
        UnlockMenu.Open();
    }

    public override void OnBackPressed()
    {
        Application.Quit();
    }

    public void OnClickLevelSelect(bool b)
    {
        levelSelectPanel.SetActive(b);
    }

    #region ġƮ
    public void OnOffCheetPanel(bool b)
    {
        cheetPanel.SetActive(b);
    }
    public void ResetSaveData()
    {
        SaveSystem.FirstSave();
        SaveFile.ReloadSaveData();
    }

    public void AllUnlock()
    {
        SaveSystem.AllUnlock();
        SaveFile.ReloadSaveData();
    }

    public void GetResource()
    {
        SaveData saveData = SaveFile.GetSaveData();
        saveData.crystal += 100;
        saveData.prestige += 100;
        SaveFile.SaveGame();
    }
    #endregion
}
