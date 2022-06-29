using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInfos : MonoBehaviour
{
    public int index;
    public int maxIndex;

    public List<GameObject> infoList;

    public GameObject leftBtn;
    public GameObject rightBtn;

    private void OnEnable()
    {
        maxIndex = infoList.Count - 1;

        index = 0;

        ShowInfo();
    }

    public void OnClickLeft()
    {
        index = Mathf.Max(--index, 0);

        ShowInfo();
    }

    public void OnClickRight()
    {
        index = Mathf.Min(++index, maxIndex);

        ShowInfo();
    }

    private void ShowInfo()
    {
        foreach(GameObject info in infoList)
        {
            info.SetActive(false);
        }

        infoList[index].SetActive(true);

        leftBtn.SetActive(true);
        rightBtn.SetActive(true);
        if (index == 0)
        {
            leftBtn.SetActive(false);
        }
        else if(index == maxIndex)
        {
            rightBtn.SetActive(false);
        }
    }
}
