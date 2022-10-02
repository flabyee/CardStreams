using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageMenu : Menu<VillageMenu>
{
    public IntValue stageValue;
    public GameObject cheetPanel;
    public GameObject levelPanel;

    public void OnClickLevelSelect(bool b)
    {
        levelPanel.SetActive(b);
    }

    public void OnPlay(int i)
    {
        stageValue.RuntimeValue = i;
        LoadingSceneManager.LoadScene("Village");
    }

    public void OnClickVillage()
    {
        LoadingSceneManager.LoadScene("Village");
    }

    public void OnClickEquip()
    {
        EquipMenu.Open();
    }

    public void OnClickUnlock()
    {
        UnlockMenu.Open();
    }

    public void OnClickTutorial()
    {
        stageValue.RuntimeValue = 0;
        LoadingSceneManager.LoadScene("SampleScene");
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
