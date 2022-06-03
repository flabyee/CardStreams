using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetRewardCard : MonoBehaviour
{
    [SerializeField] Image cardIconImage;
    [SerializeField] TextMeshProUGUI cardNameText;
    // [SerializeField] TextMeshProUGUI cardDescriptionText; 나중에

    /// <summary> 던질카드 Init </summary>
    /// <param name="icon">카드 아이콘</param>
    /// <param name="description">카드 설명</param>
    public void Init(Sprite icon, string cardName)
    {
        cardIconImage.sprite = icon;
        cardNameText.text = cardName;
        // cardDescriptionText.text = description;
    }
}