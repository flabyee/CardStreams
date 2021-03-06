using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup[] tutorialImages;
    [SerializeField] IntValue stageNum;

    private int currentIndex; // 현재 보여지는 튜토리얼이미지

    private List<int> showIndexList = new List<int>(); // 보여진것들(안켜줌 2번은)
    private void Awake()
    {
    }

    private void Start()
    {
        // 튜토리얼 스테이지라면 튜토리얼을 키자
        if (stageNum.RuntimeValue == 0)
        {
            GameManager.Instance.ShowTuTorialEvent += ShowTutorial;
            ShowTutorial(0);
        }
    }

    public void ShowHide(bool value) // 독립적인씬이면 사실상쓸일없음
    {
        gameObject.SetActive(value);
    }

    public void ShowTutorial(int index)
    {
        //if(showIndexList.Contains(index)) // 이미 한번 보여준거면 x
        //{
        //    return;
        //}

        showIndexList.Add(index);
        HideTutorial();

        tutorialImages[index].blocksRaycasts = true;
        tutorialImages[index].interactable = true;
        tutorialImages[index].DOFade(1f, 0.5f);

        currentIndex = index;

        GameManager.Instance.blurController.SetActive(true);
    }
    
    public void HideTutorial()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].alpha = 0;
            tutorialImages[i].blocksRaycasts = false;
            tutorialImages[i].interactable = false;
        }

        GameManager.Instance.blurController.SetActive(false);
    }
}
