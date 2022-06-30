using DG.Tweening;
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
    [SerializeField] float cardReverseTime = 1f;
    [SerializeField] GameObject _cgObject;

    private List<RewardSO> canChooseRewardList = new List<RewardSO>();
    private List<RewardListSO> loopRewardList = new List<RewardListSO>();
    private int loopCount = 0; // �˾Ƽ� ���ư�, ���߿� ����ȭ ��?��
    private int rewardCount = 0; // ��� ���� ����
    private CanvasGroup _cg;
    private bool isGetCard = false; // ������ ȹ���ϰ� �ִ���

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

    public void SetReward() // ������
    {
        isGetCard = false;

        for (int i = 0; i < randomRewardNums.Length; i++) // ���� 012 ����
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

        loopCount++; // ��������� ��������Ƚ�� + 1
        SetActiveChoose(true);
    }

    /// <summary> �����ư �������� �۵� </summary>
    /// <param name="index">���� ��ư</param>
    private void GetReward(int index)
    {
        int chooseNumber = randomRewardNums[index]; // ������ = ������������ index��°

        RewardSO reward = canChooseRewardList[chooseNumber]; // ������ = 3���߿��ϳ�[������]
        

        // ����ȹ��â on
        SetActiveChoose(false);
        SetActiveGet(true);

        int cardCount = 0;

        if (reward.goldReward > 0) // ��� ������ �ִٸ� ī���ϳ�����
        {
            // ī�� �ϳ� Init�ϰ� ������ ��UI�� �������ϱ�
            getCards[cardCount].GoldInit(reward.goldReward, cardReverseTime);
            cardCount++;
        }

        if (reward.allHealReward == true)
        {
            // ī�� �ϳ� Init�ϰ� ������ �÷��̾���Ʈ�� �������ϱ�
            getCards[cardCount].HealInit(cardReverseTime);
            cardCount++;
        }

        int statCount = cardCount;

        for (int i = 0; i < reward.cardReward.Length; i++) // reward Ư��ī����� �� �Ծ���ϴϱ� i = 0���� ���������ؼ� �ٸԱ�
        {
            SpecialCardSO card = reward.cardReward[i]; // i��° ����
            getCards[i + statCount].CardInit(card, cardReverseTime); // ������ i + statCount ���������� (ex : 2��°����, statCount 1 = 3��°ī�忡 ��)
            cardCount++; // �׷��� cardCount�� �������Ѿ���
        }

        rewardCount = cardCount; // ������� ���󰳼�

        for (; cardCount < getCards.Length; cardCount++) // ����ƴ� ī�����
        {
            getCards[cardCount].ResetReward(); // �ʱ�ȭ�ϰ� ��� off
        }
    }

    public void PressOKButton() // ��ư���δ���
    {
        if (isGetCard == true) return;

        isGetCard = true;
        bool isAllGet = true; // ����ī�� �� ����������?

        for (int i = 0; i < rewardCount; i++) // Ȯ��
        {
            RewardCard card = getCards[i];

            if (card.isget == false)
            {
                isAllGet = false;
                break;
            }
        }

        Sequence seq = DOTween.Sequence();

        if (isAllGet == false) // �� �ȵ������ٸ�
        {
            seq.AppendCallback(() =>
            {
                for (int i = 0; i < rewardCount; i++)
                {
                    RewardCard card = getCards[i];
                    if (card.isget == false) card.PressButton(); // �ȵ������� ������
                }
            });

            seq.AppendInterval(cardReverseTime);
        }

        seq.AppendCallback(() => // ��� ī�� ������ ������ ȹ��(�����°� ������) + �г� ����
        {
            SetActiveGet(false);
            Hide();

            for (int i = 0; i < rewardCount; i++)
            {
                Debug.Log("getCards : " + i);
                RewardCard card = getCards[i];
                card.GetReward();
            }
        });
    }
}
