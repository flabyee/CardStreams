using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartingPanel : MonoBehaviour
{
    [SerializeField] SpecialCardSO[] startingCards;
    [SerializeField] Button[] startingButtons;

    [SerializeField] GameObject itemInfo;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemExplainText;

    public IntValue stageNumValue;

    private CanvasGroup _cg;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (stageNumValue.RuntimeValue == 0)
            return;

        _cg.alpha = 1;
        _cg.blocksRaycasts = true;
        _cg.interactable = true;

        itemInfo.SetActive(false);

        for (int i = 0; i < startingButtons.Length; i++)
        {
            // ÀÌ¹ÌÁö¹Ù²Ù±â
            startingButtons[i].image.sprite = startingCards[i].sprite;
        }

        GameManager.Instance.blurController.SetActive(true);
    }

    public void PressStarting(int startingIndex)
    {
        // Á¤ÇØÁø ½ºÆä¼ÈÄ«µå È¹µæ
        GameManager.Instance.handleController.AddSpecial(startingCards[startingIndex].id);

        gameObject.SetActive(false);

        GameManager.Instance.canNext = true;

        GameManager.Instance.blurController.SetActive(false);
    }

    public void EnterStarting(int startingIndex)
    {
        if(itemInfo.activeSelf == false)
        {
            itemInfo.SetActive(true);
        }

        itemImage.sprite = startingCards[startingIndex].sprite;
        itemNameText.text = startingCards[startingIndex].specialCardName;
        itemExplainText.text = startingCards[startingIndex].tooltip;
    }
}
