using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu<MainMenu>
{
    public void OnClickPlay()
    {
        VillageMenu.Open();
    }

    public override void OnBackPressed()
    {
        Application.Quit();
    }
}
