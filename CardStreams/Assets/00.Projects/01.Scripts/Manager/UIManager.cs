using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject[] tutorialArr;
    private int tutorialIndex;


    private void Start()
    {
        foreach(GameObject item in tutorialArr)
        {
            item.SetActive(false);
        }
    }

    public void OnTutorial()
    {
        tutorialPanel.SetActive(true);
        tutorialIndex = 0;

        foreach (GameObject item in tutorialArr)
        {
            item.SetActive(false);
        }

        tutorialArr[tutorialIndex].SetActive(true);
    }

    public void OffTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void OnTutorialLeft()
    {
        tutorialIndex = Mathf.Clamp(tutorialIndex - 1, 0, tutorialArr.Length - 1);

        foreach (GameObject item in tutorialArr)
        {
            item.SetActive(false);
        }

        tutorialArr[tutorialIndex].SetActive(true);
    }

    public void OnTutorialRight()
    {
        tutorialIndex = Mathf.Clamp(tutorialIndex + 1, 0, tutorialArr.Length - 1);

        foreach (GameObject item in tutorialArr)
        {
            item.SetActive(false);
        }

        tutorialArr[tutorialIndex].SetActive(true);
    }
}
