using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandleCardTooltip : MonoBehaviour
{
    public static HandleCardTooltip Instance;

    [SerializeField] Image iconImage;
    [SerializeField] Image backgroundColorImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] float veryLowHeight = -10f; // 카드 맨아래부분이 수치 이하로 내려가면 알아보기힘든값, 보통 -10 ~ -20

    private RectTransform _rectTrm;

    private void Awake()
    {
        // 싱글톤
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        // 외 다른거
        _rectTrm = GetComponent<RectTransform>();
    }

    /// <summary> 일반카드용 Init + Show </summary>
    public void Show(Vector3 pos, Sprite icon, string cardName, Color bgColor, int power)
    {
        SetTooltipPos(pos);

        // transform.rotation = rot; 카드 회전 안바꾸게변경
        powerText.text = power.ToString();

        iconImage.sprite = icon;
        nameText.text = cardName;
        backgroundColorImage.color = bgColor;
        descriptionText.text = "";

        gameObject.SetActive(true);
    }
    
    /// <summary> 특수카드용 Init + Show </summary>
    public void Show(Vector3 pos, Sprite icon, string cardName, Color bgColor, string description)
    {
        SetTooltipPos(pos);

        // transform.rotation = rot; 카드 회전 안바꾸게변경
        powerText.text = "";

        iconImage.sprite = icon;
        nameText.text = cardName;
        backgroundColorImage.color = bgColor;
        descriptionText.text = description;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetTooltipPos(Vector3 pos)
    {
        transform.position = pos;
        if (_rectTrm.anchoredPosition.y - _rectTrm.rect.height / 2 <= veryLowHeight) // 중앙y - 높이/2 < 너무낮은수치(0), 즉 카드 맨아래부분의 y가 너무 작아서 카드가 안보인다면
        {
            _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, _rectTrm.rect.height / 2 + veryLowHeight);
        }
    }
}
