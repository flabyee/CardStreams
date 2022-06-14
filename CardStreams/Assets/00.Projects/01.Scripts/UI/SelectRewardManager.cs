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

    [SerializeField] IntValue hpValue;
    [SerializeField] IntValue goldValue;
    [SerializeField] EventSO goldValueChanged;
    [SerializeField] EventSO playerValueChanged;

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

        for (int i = 0; i < reward.cardReward.Length; i++)
        {
            SpecialCardSO card = reward.cardReward[i];
            getCards[i].Init(card);
        }

        for (int i = reward.cardReward.Length; i < getCards.Length; i++)
        {
            getCards[i].ResetReward();
        }

        if (reward.goldReward > 0)
        {
            goldValue.RuntimeValue += reward.goldReward;
            goldValueChanged.Occurred();

            GoldAnimManager.Instance.CreateCoin(reward.goldReward, transform.position);
            Effects.Instance.TriggerBlock(transform.position);
        }



        // 체력 모두 회복
        if (reward.allHealReward == true)
        {
            hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
            playerValueChanged.Occurred();
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
