using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HandleCardTooltip : MonoBehaviour
{
    public static HandleCardTooltip Instance;

    [Header("Basic")]
    [SerializeField] GameObject basicObj;
    [SerializeField] Image iconImage_basic;
    [SerializeField] Image backgroundColorImage_basic;
    [SerializeField] TextMeshProUGUI nameText_basic;
    [SerializeField] TextMeshProUGUI powerText_basic;
    [SerializeField] TextMeshProUGUI descriptionText_basic;

    [Header("Special")]
    [SerializeField] GameObject specialObj;
    [SerializeField] Image iconImage_special;
    [SerializeField] TextMeshProUGUI nameText_special;
    [SerializeField] TextMeshProUGUI descriptionText_special;

    [Header("Build")]
    [SerializeField] GameObject buildObj;
    [SerializeField] Image iconImage_build;
    [SerializeField] TextMeshProUGUI nameText_build;
    [SerializeField] TextMeshProUGUI descriptionText_build;






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

        Hide();
    }

    /// <summary> 일반카드용 Init + Show </summary>
    public void ShowBasic(Vector3 pos, Sprite icon, string cardName, Color bgColor, int power)
    {
        SetTooltipPos(pos);

        powerText_basic.text = power.ToString();
        iconImage_basic.sprite = icon;
        nameText_basic.text = cardName;
        backgroundColorImage_basic.color = bgColor;
        descriptionText_basic.text = "";

        basicObj.SetActive(true);
    }
    
    /// <summary> 특수카드용 Init + Show </summary>
    public void ShowSpecial(Vector3 pos, Sprite icon, string cardName, string description)
    {
        SetTooltipPos(pos);

        // transform.rotation = rot; 카드 회전 안바꾸게변경
        iconImage_special.sprite = icon;
        nameText_special.text = cardName;
        descriptionText_special.text = description;

        specialObj.SetActive(true);
    }
    
    /// <summary> 특수카드용 Init + Show </summary>
    public void ShowBuild(Vector3 pos, Sprite icon, string cardName, string description)
    {
        SetTooltipPos(pos);

        // transform.rotation = rot; 카드 회전 안바꾸게변경
        iconImage_build.sprite = icon;
        nameText_build.text = cardName;
        descriptionText_build.text = description;

        buildObj.SetActive(true);
    }

    public void Hide()
    {
        basicObj.SetActive(false);
        specialObj.SetActive(false);
        buildObj.SetActive(false);
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
