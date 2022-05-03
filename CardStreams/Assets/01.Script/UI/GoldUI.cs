using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    public IntValue goldValue;

    public void ApplyGoldToText()
    {
        goldText.text = $"gold : {goldValue.RuntimeValue}";
    }
}
