using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerValueUI : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI swordValueText;
    public TextMeshProUGUI sheildValueText;

    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;

    //public StatPanel hpStatPanel;
    //public StatPanel swordStatPanel;
    //public StatPanel shieldStatPanel;

    public void ApplyValueToText()
    {
        // hpStatPanel.A(hpValue.RuntimeValue);

        hpText.text = $"<sprite=2>{hpValue.RuntimeValue}";
        swordValueText.text = $"<sprite=0>{swordValue.RuntimeValue}";
        sheildValueText.text = $"<sprite=1>{shieldValue.RuntimeValue}";
    }
}
