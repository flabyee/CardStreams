using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandleManager : MonoBehaviour
{
    [HideInInspector] public List<CardData> originDeck = new List<CardData>();
    [HideInInspector] public List<CardData> deck = new List<CardData>();

    [Header("UI")]
    public DropArea handleDropArea;
    public RectTransform handleTrm;

    public DropArea buildHandleDropArea;
    public RectTransform buildHandleTrm;

    public DropArea shopDropArea;

    [Header("Card")]
    public GameObject cardPrefab;

    [Header("Build")]
    public GameObject buildPrefab;

    [Header("SpecialCard")]
    public GameObject specialCardPrefab;

    [Header("System")]
    private int handleCount = 3;    // �տ� ������� ī�� �ִ��
    private bool isDeckShuffle;

    private int deckValueAmount;        
    private int deckValueIncreaseAmount;
    private float deckValueIncreaseMultipication;

    private int maxValue;

    private void Awake()
    {
        
    }

    private void Start()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();

        handleCount = stageData.moveCount;
        isDeckShuffle = stageData.isDeckShuffle;

        deckValueAmount = stageData.firstDeckValueAmount;
        deckValueIncreaseAmount = stageData.deckValueIncreaseAmount;
        deckValueIncreaseMultipication = stageData.deckValueIncreaseMultipication;
        maxValue = 5;

        DeckMake();

        DeckShuffle(deck);
    }

    // 0,1   2,3   4,5,6,   7,8,9,10,11
    private void DeckMake()
    {
        deck.Clear();
        originDeck.Clear();


        // originDeck �����
        for(int i = 0; i < 24; i++)
        {
            switch(i % 12)
            {
                case 0:
                case 1:
                    originDeck.Add(new CardData(CardType.Sword, UnityEngine.Random.Range(2, maxValue), DropAreaType.feild));
                    break;
                case 2:
                case 3:
                    originDeck.Add(new CardData(CardType.Sheild, UnityEngine.Random.Range(2, maxValue), DropAreaType.feild));
                    break;
                case 4:
                case 5:
                case 6:
                    originDeck.Add(new CardData(CardType.Potion, UnityEngine.Random.Range(2, maxValue), DropAreaType.feild));
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    originDeck.Add(new CardData(CardType.Monster, UnityEngine.Random.Range(2, 5), DropAreaType.feild));
                    break;
            }
        }

        int pValue = 0;
        int mValue = 0;

        foreach(CardData cardData in originDeck)
        {
            if (cardData.cardType != CardType.Monster)
                pValue += cardData.value;
            else
                mValue += cardData.value;
        }

        int deckValue = pValue - mValue;
        int randomIndex = 0;

        while (deckValue != deckValueAmount)
        {
            randomIndex = UnityEngine.Random.Range(0, 24);
            // pValue�� �� ũ�ٸ�
            if (deckValue > deckValueAmount)
            {
                // �÷��̾� ī�带 ���̰ų� �� ī�带 �ø���
                if (originDeck[randomIndex].cardType != CardType.Monster)
                {
                    if (originDeck[randomIndex].value > 1)
                        originDeck[randomIndex].value--;
                    else
                        deckValue++;
                }
                else
                {
                    if (originDeck[randomIndex].value < maxValue)
                        originDeck[randomIndex].value++;
                    else
                        deckValue++;
                }

                deckValue--;
            }
            // mValue�� �� ũ�ٸ�
            else
            {
                // �÷��̾� ī�带 �ø��ų� �� ī�带 ���δ�
                if (originDeck[randomIndex].cardType != CardType.Monster)
                {
                    if (originDeck[randomIndex].value < maxValue)
                        originDeck[randomIndex].value++;
                    else
                        deckValue--;
                }
                else
                {
                    if (originDeck[randomIndex].value > 1)
                        originDeck[randomIndex].value--;
                    else
                        deckValue--;
                }


                deckValue++;
            }
        }

        DeckShuffle(originDeck);

        // deck�� �߰��ϰ� ����
        foreach(CardData cardData in originDeck)
        {
            deck.Add(cardData);
        }
        DeckShuffle(deck);
    }

    public void CardRerollAdd(GameObject dragbleCardObj)
    {
        CardPower cardPower = dragbleCardObj.GetComponent<CardPower>();

        deck.Add(new CardData(cardPower.cardType, cardPower.value, cardPower.dropAreaType));

        DeckShuffle(deck, true);
    }

    public void CardAdd(CardData cardData)
    {
        // To Do : ������ ���� �߰��ϰ� �ڷ� �б�
        deck.Add(cardData);
    }

    private void DeckShuffle(List<CardData> cardList, bool isNew = false)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int j = UnityEngine.Random.Range(0, cardList.Count);
            CardData temp = cardList[i];
            cardList[i] = cardList[j];
            cardList[j] = temp;
        }

        if (isNew == false)
        {
            
        }
        else
        {
            // To Do : ī�� �ϳ��� �߰��Ҷ��� ��ü �迭�� �������� �� ī�� �ϳ��� �߰��� �����ֱ�
        }
    }

    private CardData GetCardData()
    {
        if (deck.Count > 0)
        {
            CardData cardData;

            cardData = deck[0];

            //usedDeck.Add(deck[0]);  // ����� ī�� ������ �߰�

            deck.RemoveAt(0);

            return cardData;
        }
        else
        {
            foreach (CardData cardData in originDeck)
            {
                deck.Add(cardData);
            }

            DeckShuffle(deck);

            return GetCardData();
        }
    }

    // ���� ī���� ������ ��ȯ�Ѵ�
    public CardData DrawCard()
    {
        CardData cardData = GetCardData();

        if(cardData != null)
        {
            GameObject cardObj = Instantiate(cardPrefab, handleTrm);
            DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();

            dragbleCard.SetDroppedArea(handleDropArea);
            dragbleCard.originDropArea = handleDropArea;

            dragbleCard.SetData_Feild(cardData.cardType, cardData.value);
        }
        else
        {
            Debug.LogError("ī�带 ������ ����?");
        }

        if(cardData != null)
        {
            return cardData;

        }
        else
        {
            return null;
        }
    }

    // �÷��̾� ī�带 �̴´�
    public CardData DrawCardP()
    {
        CardData cardData = GetCardData();

        if (cardData == null)
            Debug.LogError("GetCardData is null");

        // ���Ͷ�� �ٽ� �̱�
        if (cardData.cardType == CardType.Monster)
        {
            CardAdd(cardData);

            foreach(CardData deckCardData in deck)
            {
                // �÷��̾� ī�尡 �ϳ��� �ִٸ�
                if(deckCardData.cardType != CardType.Monster)
                {
                    return DrawCardP();
                }
                else
                {
                    DeckShuffle(originDeck);
                    foreach(CardData item in originDeck)
                    {
                        deck.Add(item);
                    }

                    DeckShuffle(deck);

                    return DrawCardP();
                }
            }

        }
        else
        {
            GameObject cardObj = Instantiate(cardPrefab, handleTrm);
            DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();

            dragbleCard.SetDroppedArea(handleDropArea);
            dragbleCard.originDropArea = handleDropArea;

            dragbleCard.SetData_Feild(cardData.cardType, cardData.value);
        }

        return cardData;
    }

    // ���͸� �̴´�
    public CardData DrawCardM()
    {
        CardData cardData = GetCardData();

        if (cardData == null)
            Debug.LogError("GetCardData is null");

        // �÷��̾� ī���� �ٽ� �̱�
        if(cardData.cardType != CardType.Monster)
        {
            CardAdd(cardData);

            foreach (CardData deckCardData in deck)
            {
                // ���Ͱ� �ϳ��� �ִٸ�
                if (deckCardData.cardType == CardType.Monster)
                {
                    return DrawCardM();
                }
            }

            // �ϳ��� ������ �� �߰��ϰ� �ٽ� �̱�
            DeckShuffle(originDeck);
            foreach (CardData item in originDeck)
            {
                deck.Add(item);
            }

            DeckShuffle(deck);

            return DrawCardM();
        }
        else
        {
            GameObject cardObj = Instantiate(cardPrefab, handleTrm);
            DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();

            dragbleCard.SetDroppedArea(handleDropArea);
            dragbleCard.originDropArea = handleDropArea;

            dragbleCard.SetData_Feild(cardData.cardType, cardData.value);
        }

        return cardData;
    }

    private void DrawBuild()
    {
        SaveData saveData = DataManager.Instance.saveData;

        foreach(BuildData buildData in saveData.buildDataList)
        {
            if(buildData.isUnlock == true)
            {
                int count = buildData.haveAmount;
                for (int i = 0; i < count; i++)
                {
                    GameObject buildObj = Instantiate(buildPrefab, buildHandleTrm);

                    // build ���� �ʱ�ȭ

                    Build build = buildObj.GetComponent<Build>();
                    build.Init(DataManager.Instance.GetBuildSO(buildData.id));


                    // dragble ���� �ʱ�ȭ
                    DragbleCard dragbleCard = buildObj.GetComponent<DragbleCard>();

                    dragbleCard.SetDroppedArea(buildHandleDropArea);
                    dragbleCard.originDropArea = buildHandleDropArea;

                    dragbleCard.SetData_Build();

                    buildData.haveAmount--;
                }
            }
        }
    }

    private void DrawSpecialCard()
    {
        SaveData saveData = DataManager.Instance.saveData;

        foreach (SpecialCardData specialCardData in saveData.speicialCardDataList)
        {
            if(specialCardData.isUnlock == true)
            {
                int count = specialCardData.haveAmount;

                for (int i = 0; i < count; i++)
                {
                    GameObject specialCardObj = Instantiate(specialCardPrefab, buildHandleTrm);
                    DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

                    // specialCard ���� �ʱ�ȭ
                    SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                    specialCard.Init(DataManager.Instance.GetSpecialCardSO(specialCardData.id));

                    // dragble ���� �ʱ�ȭ
                    dragbleCard.SetDroppedArea(buildHandleDropArea);
                    dragbleCard.originDropArea = buildHandleDropArea;

                    dragbleCard.SetData_SpecialCard();

                    specialCardData.haveAmount--;
                }
            }
        }
    }

    private void UseCard()
    {
        //Debug.Log("use card");
        //haveCardCount--;
        //useCount++;
        //if (drawCount == 17)
        //{
        //    Debug.Log("��� ī�� ���� ��ư�� ���� ���� ������ ������");
        //}
        //else
        //{
        //    DrawCard();
        //}
    }

    // turnStart or moveEnd ���� �ϴ� draw

    public void DrawCardWhenBeforeMove()
    {
        Stack<DragbleCard> sellCardStack = new Stack<DragbleCard>();
        // �̱����� �����ִ� ī�尡 �ִٸ� ����
        for (int i = 0; i < handleTrm.childCount; i++)
        {
            GameObject card = handleTrm.GetChild(i).gameObject;
            card.SetActive(false);
            sellCardStack.Push(card.GetComponent<DragbleCard>());
        }

        while(sellCardStack.Count != 0)
        {
            Destroy(sellCardStack.Pop().gameObject);
        }

        int pCount = 0;
        int mCount = 0;
        for (int i = 0; i < handleCount - 1; i++)
        {
            CardData cardData = DrawCard();

            switch(cardData.cardType)
            {
                case CardType.Sword:
                case CardType.Sheild:
                case CardType.Potion:
                    pCount++;  break;
                case CardType.Monster:
                    mCount++; break;
            }
        }

        if(pCount == handleCount - 1)
        {
            DrawCardM();
        }
        else if(mCount == handleCount - 1)
        {
            DrawCardP();
        }
        else
        {
            DrawCard();
        }
    }

    public void DrawBuildAndSpecialWhenTurnStart()
    {
        //Stack<DragbleCard> sellCardStack = new Stack<DragbleCard>();
        //// �̱����� �����ִ� ī�尡 �ִٸ� ����
        //for (int i = 0; i < buildHandleTrm.childCount; i++)
        //{
        //    GameObject card = buildHandleTrm.GetChild(i).gameObject;
        //    card.SetActive(false);
        //    sellCardStack.Push(card.GetComponent<DragbleCard>());
        //}

        //while (sellCardStack.Count != 0)
        //{
        //    Destroy(sellCardStack.Pop().gameObject);
        //}

        DrawBuild();
        DrawSpecialCard();
    }

    public void TurnEnd()
    {
        
        deck.Clear();

        deckValueAmount -= deckValueIncreaseAmount;
        maxValue = Mathf.RoundToInt((float)maxValue * deckValueIncreaseMultipication);

        Debug.Log(maxValue);

        deckValueIncreaseAmount = Mathf.RoundToInt((float)deckValueIncreaseAmount * deckValueIncreaseMultipication);

        DeckMake();
    }
}
