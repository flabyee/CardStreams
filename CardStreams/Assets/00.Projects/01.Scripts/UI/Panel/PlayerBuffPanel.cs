using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuffPanel : MonoBehaviour
{
    [SerializeField] PlayerBuffSlot[] buffSlots;
    [SerializeField] GameObject buffInfoPanel;
    [SerializeField] Image buffImage;
    [SerializeField] TextMeshProUGUI buffNameText;
    [SerializeField] TextMeshProUGUI buffTooltipText;

    private List<Buff> playerBuffList = new List<Buff>();
    private CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        for (int i = 0; i < buffSlots.Length; i++)
        {
            buffSlots[i].buffPanel = this;
        }
        HideBuffInfo();
    }

    public void Show()
    {
        UpdatePanel();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void Hide()
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void ShowBuffInfo(Buff buff) // �г��� ���� ������ �����մϴ�
    {
        Debug.Log(buff);
        buffNameText.text = buff.buffName;
        buffImage.sprite = buff.buffIcon;
        buffTooltipText.text = buff.buffTooltip; // ���� ������ �־���ϴϱ� ���̵� �߰��ϱ�
        buffInfoPanel.SetActive(true);
    }

    public void HideBuffInfo()
    {
        buffInfoPanel.SetActive(false);
        buffNameText.text = "";
        buffImage.sprite = null;
        buffTooltipText.text = ""; // ���� ������ �־���ϴϱ� ���̵� �߰��ϱ�
    }

    public void AddBuff(Buff buff) // �������â�� �ش� ���� �߰�
    {
        Show();
        playerBuffList.Add(buff);
        UpdatePanel();
    }

    public void RemoveBuff(Buff buff) // �������â���� �ش� ���� ����
    {
        playerBuffList.Remove(buff);
        if (playerBuffList.Count <= 0) Hide();
        UpdatePanel();
    }

    public void UpdateBuffTime() // �� ������ ��Ÿ���� �������ݴϴ� : EventListener
    {
        foreach (PlayerBuffSlot slot in buffSlots)
        {
            slot.UpdateBuffTime();
        }
    }

    private void UpdatePanel() // ���� �����ܵ��� �������ݴϴ�
    {
        int i = 0;

        for (; i < playerBuffList.Count; i++) // ���� ��������
        {
            buffSlots[i].SetBuff(playerBuffList[i]);
        }

        for (; i < buffSlots.Length; i++) // ������ �ƴ� ĭ�� ���
        {
            buffSlots[i].SetBuff(null);
        }
    }
}