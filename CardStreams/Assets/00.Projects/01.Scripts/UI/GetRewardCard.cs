using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetRewardCard : MonoBehaviour
{
    [SerializeField] Image cardIconImage;
    [SerializeField] TextMeshProUGUI cardNameText;
    // [SerializeField] TextMeshProUGUI cardDescriptionText; ���߿�

    /// <summary> ����ī�� Init </summary>
    /// <param name="icon">ī�� ������</param>
    /// <param name="description">ī�� ����</param>
    public void Init(Sprite icon, string cardName)
    {
        cardIconImage.sprite = icon;
        cardNameText.text = cardName;
        // cardDescriptionText.text = description;
    }
}