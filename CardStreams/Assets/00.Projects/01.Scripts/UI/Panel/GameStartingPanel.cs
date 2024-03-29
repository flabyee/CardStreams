using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartingPanel : Panel
{
    [SerializeField] SpecialCardSO[] startingCards;
    [SerializeField] Button[] startingButtons;

    [SerializeField] GameObject itemInfo;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemExplainText;

    public IntValue stageNumValue;

    private void Start()
    {
        if (stageNumValue.RuntimeValue == 0)
            return;

        Show();

        itemInfo.SetActive(false);

        for (int i = 0; i < startingButtons.Length; i++)
        {
            // �̹����ٲٱ�
            startingButtons[i].image.sprite = startingCards[i].sprite;
        }

        GameManager.Instance.blurController.SetActive(true);
    }

    public void PressStarting(int startingIndex)
    {
        // ������ �����ī�� ȹ��
        GameManager.Instance.handleController.AddSpecial(startingCards[startingIndex].id);
        GameManager.Instance.canNextLoop = true;

        gameObject.SetActive(false);


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
