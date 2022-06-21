using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup[] tutorialImages;
    private int currentIndex; // ���� �������� Ʃ�丮���̹���

    private void Awake()
    {
    }

    private void Start()
    {
        GameManager.Instance.ShowTuTorialEvent += ShowTutorial;
        Debug.Log("?");
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            Debug.Log("??");
            HideTutorial(i);
        }
    }

    public void ShowHide(bool value) // �������ξ��̸� ��ǻ��Ͼ���
    {
        gameObject.SetActive(value);
    }

    public void ShowTutorial(int index)
    {
        HideTutorial(currentIndex);

        tutorialImages[index].DOFade(1f, 0.5f);
        tutorialImages[index].blocksRaycasts = true;
        tutorialImages[index].interactable = true;

        currentIndex = index;
    }
    
    private void HideTutorial(int index)
    {
        tutorialImages[index].DOFade(0f, 0.5f);
        tutorialImages[index].blocksRaycasts = false;
        tutorialImages[index].interactable = false;
    }
}
