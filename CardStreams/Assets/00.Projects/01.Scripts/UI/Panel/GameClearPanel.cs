using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : MonoBehaviour
{
    public void OnClickTitleButton()
    {
        ResourceManager.Instance.AddResource(ResourceType.crystal, 50);
        ResourceManager.Instance.AddResource(ResourceType.prestige, 30);

        GameManager.Instance.SettingClear();

        LoadingSceneManager.LoadScene("MenuScene");
    }
}
