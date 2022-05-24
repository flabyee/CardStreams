using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildExplain : MonoBehaviour
{
    public static BuildExplain Instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        Instance = this;

        nameText.text = "";
        infoText.text = "";
    }

    public void Show(BuildSO buildSO)
    {
        nameText.text = buildSO.buildName;
        infoText.text = buildSO.tooltip;
    }

    public void Hide()
    {
        nameText.text = "";
        infoText.text = "";
    }
}
