using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu<MainMenu>
{
    public IntValue stageValue;

    public void OnClickPlay(int i)
    {
        stageValue.RuntimeValue = i;
        LoadingSceneManager.LoadScene("SampleScene");
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
}
