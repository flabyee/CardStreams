using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : Panel
{
    public void ReturnMenu()
    {
        Debug.Log("���");
        LoadingSceneManager.LoadScene("MenuScene");
    }
}
