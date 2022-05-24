using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardPanel : MonoBehaviour
{
    [SerializeField] RewardCard[] rewardCards;
    private CanvasGroup _cg;

    private List<RewardListSO> loopRewardList;
    private int loopCount = 0; // 알아서 돌아감, 나중에 동기화 필?요

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();

        Hide();

        loopRewardList = Resources.Load<LoopRewardListSO>("LoopRewardList").loopList;
    }

    public void Show()
    {
        _cg.alpha = 1;
        _cg.blocksRaycasts = true;
        _cg.interactable = true;
    }

    public void Hide()
    {
        _cg.alpha = 0;
        _cg.blocksRaycasts = false;
        _cg.interactable = false;
    }

    public void GetSpecialCard(SpecialCardSO so)
    {
        SaveData saveData = SaveSystem.Load();
        saveData.speicialCardDataList[so.id].haveAmount++;
        SaveSystem.Save(saveData);
    }

    public void InitReward()
    {
        for (int i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].gameObject.SetActive(false);
        }

        int rewardCount = loopRewardList[loopCount].rewardList.Count;

        foreach (var item in collection)
        {

        }

        loopCount++;
    }
}
