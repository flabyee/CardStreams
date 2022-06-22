using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoopCountUI : MonoBehaviour
{
    public TextMeshProUGUI loopCountText;
    public IntValue loopCount;

    public void UpdateText()
    {
        loopCountText.text = loopCount.RuntimeValue.ToString();

        // To Do : ���߿� �̹��� ���ư��� ����
    }
}
