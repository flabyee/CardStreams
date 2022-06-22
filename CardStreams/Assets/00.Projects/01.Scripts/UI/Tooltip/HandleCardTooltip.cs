using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleCardTooltip : MonoBehaviour
{
    public static HandleCardTooltip Instance;

    [SerializeField] Image iconImage;
    [SerializeField] Image backgroundColorImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI powerText;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public void Show(Vector3 pos, Quaternion rot, int power, Sprite icon, string cardName, Color bgColor)
    {
        transform.position = pos;
        transform.rotation = rot;
        powerText.text = power.ToString();

        iconImage.sprite = icon;
        nameText.text = cardName;
        backgroundColorImage.color = bgColor;

        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
