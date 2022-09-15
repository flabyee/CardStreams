using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveDisplayPanel : MonoBehaviour
{
    [SerializeField] Image passive1Image;
    [SerializeField] Image passive2Image;
    [SerializeField] Image passive3Image;

    [SerializeField] TextMeshProUGUI passive1Text;
    [SerializeField] TextMeshProUGUI passive2Text;
    [SerializeField] TextMeshProUGUI passive3Text;

    private int passiveCount;

    public void SetPassive(Sprite passiveIcon, string passiveName) // 일단 개대충만듬 3번 호출 ㄱ
    {
        if(passiveCount == 0)
        {
            passive1Image.sprite = passiveIcon;
            passive1Text.text = passiveName;
        }
        else if(passiveCount == 1)
        {
            passive2Image.sprite = passiveIcon;
            passive2Text.text = passiveName;
        }
        else // 2부턴걍대충해
        {
            passive3Image.sprite = passiveIcon;
            passive3Text.text = passiveName;
        }
        passiveCount++;
    }
}
