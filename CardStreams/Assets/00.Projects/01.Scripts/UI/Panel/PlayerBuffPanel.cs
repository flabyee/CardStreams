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

    public void ShowBuffInfo(Buff buff) // 패널의 버프 설명을 업뎃합니다
    {
        Debug.Log(buff);
        buffNameText.text = buff.buffName;
        buffImage.sprite = buff.buffIcon;
        buffTooltipText.text = buff.buffTooltip; // 버프 툴팁이 있어야하니까 좀이따 추가하기
        buffInfoPanel.SetActive(true);
    }

    public void HideBuffInfo()
    {
        buffInfoPanel.SetActive(false);
        buffNameText.text = "";
        buffImage.sprite = null;
        buffTooltipText.text = ""; // 버프 툴팁이 있어야하니까 좀이따 추가하기
    }

    public void AddBuff(Buff buff) // 버프목록창에 해당 버프 추가
    {
        Show();
        playerBuffList.Add(buff);
        UpdatePanel();
    }

    public void RemoveBuff(Buff buff) // 버프목록창에서 해당 버프 제거
    {
        playerBuffList.Remove(buff);
        if (playerBuffList.Count <= 0) Hide();
        UpdatePanel();
    }

    public void UpdateBuffTime() // 각 버프의 쿨타임을 갱신해줍니다 : EventListener
    {
        foreach (PlayerBuffSlot slot in buffSlots)
        {
            slot.UpdateBuffTime();
        }
    }

    private void UpdatePanel() // 버프 아이콘들을 정렬해줍니다
    {
        int i = 0;

        for (; i < playerBuffList.Count; i++) // 버프 개수까지
        {
            buffSlots[i].SetBuff(playerBuffList[i]);
        }

        for (; i < buffSlots.Length; i++) // 버프가 아닌 칸들 모두
        {
            buffSlots[i].SetBuff(null);
        }
    }
}