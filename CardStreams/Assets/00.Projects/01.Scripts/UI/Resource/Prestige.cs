using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Prestige : MonoBehaviour
{
    public TextMeshProUGUI prestigeText;

    private void Start()
    {
        UpdatePrestigeText();
    }

    public void UpdatePrestigeText() // ResourceManager Action에서 실행
    {
        prestigeText.text = ResourceManager.Instance.prestige.ToString();
    }
}
