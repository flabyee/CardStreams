using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public Slider expSlider;

    private void Start()
    {
        GameManager.Instance.player.GetExpEvent += (int level, int exp, int nextExp) =>
        {
            levelText.text = level.ToString();
            expText.text = $"{exp} / {nextExp}";
            expSlider.value = (float)exp / (float)nextExp;
        };
    }
}
