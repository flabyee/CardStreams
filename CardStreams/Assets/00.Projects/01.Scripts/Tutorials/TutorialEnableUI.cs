using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{

}

public class TutorialEnableUI : MonoBehaviour
{
    private static List<TutorialEnableUI> enableList;
    private CanvasGroup cg;

    public TutorialType[] enableTypes;


    private void Awake()
    {
        if(enableList == null)
        {
            enableList = new List<TutorialEnableUI>();
        }
        enableList.Add(this);
    }

    public static void SetEnable(List<TutorialType> setTypeList)
    {
        foreach(TutorialEnableUI item in enableList)
        {
            bool isEnable = false;
            foreach(TutorialType myType in item.enableTypes)
            {
                if(setTypeList.Contains(myType))
                {
                    isEnable = true;
                    break;
                }
            }

            item.SetEnable(isEnable);
        }
    }

    public void SetEnable(bool b)
    {
        if (b)
            cg.ignoreParentGroups = true;
        else
            cg.ignoreParentGroups = false;
    }
}
