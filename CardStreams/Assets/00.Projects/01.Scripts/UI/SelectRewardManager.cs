using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRewardManager : MonoBehaviour
{
    [SerializeField] RewardCard[] rewardCards;
    private CanvasGroup _cg;

    private List<RewardListSO> loopRewardList;
    private int loopCount = 0; // 알아서 돌아감, 나중에 동기화 필?요

    [SerializeField] GameObject _cgObject;
    [SerializeField] GameObject _getRewardCardPrefab;

    [SerializeField] GameObject handleObj;

    private void Awake()
    {
        _cg = _cgObject.GetComponent<CanvasGroup>();

        Hide();

        loopRewardList = Resources.Load<LoopRewardListSO>("LoopRewardList").loopList;

        for (int i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].selectPanel = this;
        }
    }

    public void Show()
    {
        _cg.alpha = 1;
        _cg.blocksRaycasts = true;
        _cg.interactable = true;

        InitReward();
    }

    public void Hide()
    {
        _cg.alpha = 0;
        _cg.blocksRaycasts = false;
        _cg.interactable = false;

        ResetReward();
    }

    public void InitReward()
    {
        List<RewardSO> rewardList = loopRewardList[loopCount].rewardList;

        for (int i = 0; i < rewardCards.Length; i++)
        {
            if(i >= rewardList.Count) // 보상선택카드 개수 > 보상개수 == 보상 없음, ex) 카드3개 > 보상2개면 3개째는 끄기
            {
                rewardCards[i].gameObject.SetActive(false);
                continue;
            }

            rewardCards[i].SetReward(rewardList[i], _getRewardCardPrefab, handleObj);
        }

        loopCount++;
    }

    public void ResetReward()
    {
        for (int i = 0; i < rewardCards.Length; i++)
        {
            rewardCards[i].ResetReward();
        }
    }
}
