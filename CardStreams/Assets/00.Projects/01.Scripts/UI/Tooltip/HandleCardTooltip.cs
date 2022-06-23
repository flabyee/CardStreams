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

    [SerializeField] float veryLowHeight = -10f; // ī�� �ǾƷ��κ��� ��ġ ���Ϸ� �������� �˾ƺ������簪, ���� -10 ~ -20

    private RectTransform _rectTrm;

    private void Awake()
    {
        // �̱���
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        // �� �ٸ���
        _rectTrm = GetComponent<RectTransform>();
    }

    /// <summary> �Ϲ�ī��� Init + Show </summary>
    public void Show(Vector3 pos, Sprite icon, string cardName, Color bgColor, int power)
    {
        SetTooltipPos(pos);

        // transform.rotation = rot; ī�� ȸ�� �ȹٲٰԺ���
        powerText.text = power.ToString();

        iconImage.sprite = icon;
        nameText.text = cardName;
        backgroundColorImage.color = bgColor;
        descriptionText.text = "";

        gameObject.SetActive(true);
    }
    
    /// <summary> Ư��ī��� Init + Show </summary>
    public void Show(Vector3 pos, Sprite icon, string cardName, Color bgColor, string description)
    {
        SetTooltipPos(pos);

        // transform.rotation = rot; ī�� ȸ�� �ȹٲٰԺ���
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
        if (_rectTrm.anchoredPosition.y - _rectTrm.rect.height / 2 <= veryLowHeight) // �߾�y - ����/2 < �ʹ�������ġ(0), �� ī�� �ǾƷ��κ��� y�� �ʹ� �۾Ƽ� ī�尡 �Ⱥ��δٸ�
        {
            _rectTrm.anchoredPosition = new Vector2(_rectTrm.anchoredPosition.x, _rectTrm.rect.height / 2 + veryLowHeight);
        }
    }
}
