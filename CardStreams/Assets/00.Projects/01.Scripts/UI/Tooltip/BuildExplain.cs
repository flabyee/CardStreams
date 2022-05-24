using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildExplain : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;
    public Image buildImage;

    private void Awake()
    {
        
    }

    public void Show(BuildSO buildSO)
    {
        nameText.text = buildSO.buildName;
        infoText.text = buildSO.tooltip;

        buildImage.color = new Color(0, 0, 0, 1);
        buildImage.sprite = buildSO.sprite;
    }

    public void Hide()
    {
        nameText.text = "";
        infoText.text = "";

        buildImage.color = new Color(0, 0, 0, 0); 
    }
}
