using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup[] tutorialImages;
    [SerializeField] IntValue stageNum;

    private int currentIndex; // ���� �������� Ʃ�丮���̹���

    private List<int> showIndexList = new List<int>(); // �������͵�(������ 2����)
    private void Awake()
    {
    }

    private void Start()
    {
        // Ʃ�丮�� ����������� Ʃ�丮���� Ű��
        if(stageNum.RuntimeValue == 0) GameManager.Instance.ShowTuTorialEvent += ShowTutorial;
        
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            HideTutorial(i);
        }
    }

    public void ShowHide(bool value) // �������ξ��̸� ��ǻ��Ͼ���
    {
        gameObject.SetActive(value);
    }

    public void ShowTutorial(int index)
    {
        if(showIndexList.Contains(index)) // �̹� �ѹ� �����ذŸ� x
        {
            return;
        }
        showIndexList.Add(index);
        HideTutorial(currentIndex);

        tutorialImages[index].DOFade(1f, 0.5f);
        tutorialImages[index].blocksRaycasts = true;
        tutorialImages[index].interactable = true;

        currentIndex = index;
    }
    
    private void HideTutorial(int index)
    {
        tutorialImages[index].alpha = 0;
        tutorialImages[index].blocksRaycasts = false;
        tutorialImages[index].interactable = false;
    }
}
