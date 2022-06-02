using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandleController : MonoBehaviour
{
    [HideInInspector] public List<CardData> originDeck = new List<CardData>();
    [HideInInspector] public List<CardData> deck = new List<CardData>();

    [Header("UI")]
    public DropArea handleDropArea;
    public RectTransform handleTrm;

    public DropArea buildHandleDropArea;
    public RectTransform buildHandleTrm;

    public DropArea shopDropArea;


    [Header("System")]
    private int handleCount = 3;    // 손에 들고있을 카드 최대수
    private bool isDeckShuffle;

    private int deckValueAmount;        
    private int deckValueIncreaseAmount;
    private float deckValueIncreaseMultipication;

    private int maxValue;

    public bool canStartTurn;

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
        GameManager.Instance.canStartTurn = false;

        deck.Clear();
        originDeck.Clear();

        // originDeck 만들고
        for (int i = 0; i < 24; i++)
        {
            switch (i % 12)
            {
                case 0:
                case 1:
                    originDeck.Add(new CardData(CardType.Sword, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 2:
                case 3:
                    originDeck.Add(new CardData(CardType.Sheild, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 4:
                case 5:
                case 6:
                    originDeck.Add(new CardData(CardType.Potion, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    originDeck.Add(new CardData(CardType.Monster, UnityEngine.Random.Range(2, 5)));
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
            // pValue가 더 크다면
            if (deckValue > deckValueAmount)
            {
                // 플레이어 카드를 줄이거나 몹 카드를 늘린다
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
            // mValue가 더 크다면
            else
            {
                // 플레이어 카드를 늘리거나 몹 카드를 줄인다
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

        // deck에 추가하고 셔플
        foreach(CardData cardData in originDeck)
        {
            deck.Add(cardData);
        }
        DeckShuffle(deck);

        GameManager.Instance.canStartTurn = true;
    }

    private void DeckAdd()
    {
        GameManager.Instance.canStartTurn = false;

        originDeck.Clear();

        // originDeck 만들고
        for (int i = 0; i < 24; i++)
        {
            switch (i % 12)
            {
                case 0:
                case 1:
                    originDeck.Add(new CardData(CardType.Sword, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 2:
                case 3:
                    originDeck.Add(new CardData(CardType.Sheild, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 4:
                case 5:
                case 6:
                    originDeck.Add(new CardData(CardType.Potion, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    originDeck.Add(new CardData(CardType.Monster, UnityEngine.Random.Range(2, 5)));
                    break;
            }
        }

        int pValue = 0;
        int mValue = 0;

        foreach (CardData cardData in originDeck)
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
            // pValue가 더 크다면
            if (deckValue > deckValueAmount)
            {
                // 플레이어 카드를 줄이거나 몹 카드를 늘린다
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
            // mValue가 더 크다면
            else
            {
                // 플레이어 카드를 늘리거나 몹 카드를 줄인다
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

        // deck에 추가하고 셔플
        foreach (CardData cardData in originDeck)
        {
            deck.Add(cardData);
        }
        DeckShuffle(deck);

        GameManager.Instance.canStartTurn = true;
    }

    // 리롤은 현재 사용하지않는다
    public void CardRerollAdd(GameObject dragbleCardObj)
    {
        CardPower cardPower = dragbleCardObj.GetComponent<CardPower>();

        deck.Add(new CardData(cardPower.cardType, cardPower.value));

        DeckShuffle(deck, true);
    }

    public void CardAdd(CardData cardData)
    {
        // To Do : 랜덤한 곳에 추가하고 뒤로 밀기
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
            // To Do : 카드 하나만 추가할때는 전체 배열을 섞지말고 그 카드 하나만 중간에 끼워넣기
        }
    }

    private CardData GetCardData()
    {
        if (deck.Count > 0)
        {
            CardData cardData;

            cardData = deck[0];

            //usedDeck.Add(deck[0]);  // 사용한 카드 묘지에 추가

            deck.RemoveAt(0);

            return cardData;
        }
        else
        {
            DeckAdd();

            return GetCardData();
        }
    }

    // 뽑은 카드의 정보를 반환한다
    public CardData DrawCard()
    {
        CardData cardData = GetCardData();

        if(cardData != null)
        {
            GameObject cardObj = CardPoolManager.Instance.GetBasicCard(handleTrm);
            DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();

            dragbleCard.SetDroppedArea(handleDropArea);
            dragbleCard.originDropArea = handleDropArea;

            dragbleCard.SetData_Feild(cardData.cardType, cardData.value);
        }
        else
        {
            Debug.LogError("카드를 뽑을수 없다?");
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

    private void DrawBuild()
    {
        SaveData saveData = SaveSystem.Load();

        foreach(BuildData buildData in saveData.buildDataList)
        {
            if(buildData.isUnlock == true)
            {
                int count = buildData.haveAmount;
                for (int i = 0; i < count; i++)
                {
                    GameObject buildObj = CardPoolManager.Instance.GetBuildCard(buildHandleTrm);

                    // build 관련 초기화

                    Build build = buildObj.GetComponent<Build>();
                    build.Init(DataManager.Instance.GetBuildSO(buildData.id));


                    // dragble 관련 초기화
                    DragbleCard dragbleCard = buildObj.GetComponent<DragbleCard>();

                    dragbleCard.SetDroppedArea(buildHandleDropArea);
                    dragbleCard.originDropArea = buildHandleDropArea;

                    dragbleCard.SetData_Build();

                    buildData.haveAmount--;
                }
            }
        }

        SaveSystem.Save(saveData);
    }

    private void DrawSpecialCard()
    {
        SaveData saveData = SaveSystem.Load();

        foreach (SpecialCardData specialCardData in saveData.speicialCardDataList)
        {
            if(specialCardData.isUnlock == true)
            {
                int count = specialCardData.haveAmount;

                for (int i = 0; i < count; i++)
                {
                    GameObject specialCardObj = CardPoolManager.Instance.GetSpecialCard(buildHandleTrm);
                    DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

                    // specialCard 관련 초기화
                    SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                    specialCard.Init(DataManager.Instance.GetSpecialCardSO(specialCardData.id));

                    // dragble 관련 초기화
                    dragbleCard.SetDroppedArea(buildHandleDropArea);
                    dragbleCard.originDropArea = buildHandleDropArea;

                    dragbleCard.SetData_SpecialCard();

                    specialCardData.haveAmount--;
                }
            }
        }

        SaveSystem.Save(saveData);
    }

    // turnStart or moveEnd 마다 하는 draw
    public void DrawCardWhenBeforeMove()
    {
        Stack<DragbleCard> sellCardStack = new Stack<DragbleCard>();
        // 뽑기전에 남아있는 카드가 있다면 제거
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

        for (int i = 0; i < handleCount; i++)
        {
            Debug.Log("asdf");
            DrawCard();
        }
    }

    public void DrawBuildAndSpecialWhenTurnStart()
    {
        //Stack<DragbleCard> sellCardStack = new Stack<DragbleCard>();
        //// 뽑기전에 남아있는 카드가 있다면 제거
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

        deckValueIncreaseAmount = Mathf.RoundToInt((float)deckValueIncreaseAmount * deckValueIncreaseMultipication);

        DeckMake();
    }

    public void ShowBuildHandle(bool b)
    {
        buildHandleTrm.gameObject.SetActive(b);
    }
}
