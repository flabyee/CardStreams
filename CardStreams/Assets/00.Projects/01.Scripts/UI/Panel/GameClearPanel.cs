using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    public void OnClickTitleButton()
    {
        GameManager.Instance.SettingClear();

        LoadingSceneManager.LoadScene("MenuScene");
    }
}
