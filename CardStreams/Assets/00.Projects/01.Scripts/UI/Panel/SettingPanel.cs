using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    private CanvasGroup _cg;
    private bool isOpen = false;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();

        Hide();
    }

    public void Show()
    {
        if (isOpen) // �����ִµ� ������
        {
            Hide(); // �ݾ��ָ�����
            return;
        }

        _cg.alpha = 1;
        _cg.interactable = true;
        _cg.blocksRaycasts = true;
        isOpen = true;
    }

    public void Hide()
    {
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;
        isOpen = false;
    }

    public void OnClickExit()
    {
        SaveFile.SaveGame();
        Application.Quit();
    }

    public void OnClickTitle()
    {
        SaveFile.SaveGame();
        DropArea.dropAreas.Clear();
        DontRaycastTarget.dontRaycastTargetList.Clear();

        LoadingSceneManager.LoadScene("MenuScene");
    }


}
