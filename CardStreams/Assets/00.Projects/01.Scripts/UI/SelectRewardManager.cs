using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRewardManager : MonoBehaviour
{
    [SerializeField] Button[] rewardClickButtons;
    [SerializeField] RewardCard[] getCards;

    [SerializeField] GameObject choosePanel;
    [SerializeField] GameObject getPanel;

    private List<RewardSO> canChooseRewardList = new List<RewardSO>();
    private List<RewardListSO> loopRewardList = new List<RewardListSO>();
    private int loopCount = 0; // 알아서 돌아감, 나중에 동기화 필?요

    private CanvasGroup _cg;
    [SerializeField] GameObject _cgObject;

    private int[] randomRewardNums = { 0, 1, 2 };

    private void Awake()
    {
        _cg = _cgObject.GetComponent<CanvasGroup>();

        Hide();

        loopRewardList = Resources.Load<LoopRewardListSO>("LoopRewardList").loopList;

        for (int i = 0; i < rewardClickButtons.Length; i++)
        {
            int avoidClosure = i;

            rewardClickButtons[avoidClosure].onClick.AddListener(() =>
            {
                GetReward(avoidClosure);
            });
        }
    }

    public void Show()
    {
        _cg.alpha = 1;
        _cg.blocksRaycasts = true;
        _cg.interactable = true;

        SetReward();
    }

    public void Hide()
    {
        _cg.alpha = 0;
        _cg.blocksRaycasts = false;
        _cg.interactable = false;
    }

    private void SetActiveGet(bool value)
    {
        getPanel.SetActive(value);
    }

    private void SetActiveChoose(bool value)
    {
        choosePanel.SetActive(value);
    }

    public void SetReward() // 보상설정
    {
        for (int i = 0; i < randomRewardNums.Length; i++) // 숫자 012 섞기
        {
            int rand = Random.Range(0, 3); // 0~2

            int temp = randomRewardNums[i];
            randomRewardNums[i] = randomRewardNums[rand];
            randomRewardNums[rand] = temp;
        }

        canChooseRewardList.Clear();

        foreach (var item in loopRewardList[loopCount].rewardList)
        {
            canChooseRewardList.Add(item);
        }

        SetActiveChoose(true);
    }

    /// <summary> 보상버튼 눌렀을때 작동 </summary>
    /// <param name="index">누른 버튼</param>
    private void GetReward(int index)
    {
        int chooseNumber = randomRewardNums[index]; // 고른숫자 = 랜덤돌린거의 index번째

        RewardSO reward = canChooseRewardList[chooseNumber]; // 고른보상 = 3개중에하나[고른숫자]

        // 보상획득창 on
        SetActiveChoose(false);
        SetActiveGet(true);

        int cardCount = 0;

        if(reward.goldReward > 0) // 골드 보상이 있다면 카드하나생성
        {
            // 카드 하나 Init하고 누르면 돈UI로 보내게하기
            getCards[cardCount].GoldInit(reward.goldReward);
            cardCount++;
        }

        if(reward.allHealReward == true)
        {
            // 카드 하나 Init하고 누르면 플레이어하트로 보내게하기
            getCards[cardCount].HealInit();
            cardCount++;
        }

        int statCount = cardCount;

        for (int i = 0; i < reward.cardReward.Length; i++) // reward 특수카드들을 다 먹어야하니까 i = 0으로 루프시작해서 다먹기
        {
            Debug.Log(i);
            SpecialCardSO card = reward.cardReward[i]; // i번째 보상
            getCards[i + statCount].Init(card); // 보상을 i + statCount 집어넣으면됨 (ex : 2번째보상, statCount 1 = 3번째카드에 들어감)
            cardCount++; // 그래도 cardCount는 증가시켜야함
        }

        for (; cardCount < getCards.Length; cardCount++)
        {
            getCards[cardCount].ResetReward();
        }
    }

    public void PressOKButton() // 버튼으로누름
    {
        SetActiveGet(false);
        Hide();

        for (int i = 0; i < getCards.Length; i++)
        {
            getCards[i].ResetReward();
        }
    }

    
}
