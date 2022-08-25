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
        index = Mathf.Clamp(index - 1, 0, maxIndex);
        ShowInfo();
    }

    public void OnClickRight()
    {
        index = Mathf.Clamp(index + 1, 0, maxIndex);
        ShowInfo();
    }

    private void ShowInfo()
    {
        foreach(GameObject info in infoList)
        {
            info.SetActive(false);
        }

        infoList[index].SetActive(true);

        // 왼쪽 오른쪽버튼 on/off
        bool notfirstIndex = index != 0;
        bool notLastIndex = index != maxIndex;

        leftBtn.SetActive(notfirstIndex); // 맨처음이 아니라면 on
        rightBtn.SetActive(notLastIndex); // 맨끝이 아니라면 on
    }
}
