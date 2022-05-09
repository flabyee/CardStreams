using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandleManager : MonoBehaviour
{
    public static HandleManager Instance;

    [HideInInspector] public List<CardData> originDeck = new List<CardData>();
    [HideInInspector] public List<CardData> deck = new List<CardData>();
    [HideInInspector] public List<CardData> usedDeck = new List<CardData>();    // 묘지

    public DropArea handleDropArea;
    public RectTransform handleTrm;

    public DropArea buildHandleDropArea;
    public RectTransform buildHandleTrm;

    public DropArea shopDropArea;

    public GameObject cardPrefab;
    public int haveCardCount;
    public int drawCount;

    [Header("Build")]
    public GameObject buildPrefab;


    [Header("SpecialCard")]
    public GameObject specialCardPrefab;

    public RectTransform nextBtnTrm;



    [SerializeField]private int handleCount = 3;    // 손에 들고있을 카드 최대수

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        haveCardCount = 0;

        DeckMake();

        
    }

    private void Start()
    {

    }

    private void AddAllCardData(CardType cardType, string str)
    {
        string[] strs = str.Split(' ');
        foreach (string item in strs)
        {
            deck.Add(new CardData(cardType, int.Parse(item), DropAreaType.feild));
        }
    }
    private void DeckMake()
    {
        AddAllCardData(CardType.Sword, "2 3 4");
        AddAllCardData(CardType.Sheild, "3 4 5");
        AddAllCardData(CardType.Monster, "3 3 3 3 4 4 4");
        AddAllCardData(CardType.Potion, "2 3 3 4 4");
        //AddAllCardData(CardType.Coin, "0 0 0 0 0 0");
        DeckShuffle(deck);
    }

    public void CardRerollAdd(CardType cardType, int value, DropAreaType dropAreaType)
    {
        deck.Add(new CardData(cardType, value, dropAreaType));

        DeckShuffle(deck, false);
    }

    private void DeckShuffle(List<CardData> cardList, bool isNew = false)
    {
        if(isNew == false)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                int j = UnityEngine.Random.Range(0, cardList.Count);
                CardData temp = cardList[i];
                cardList[i] = cardList[j];
                cardList[j] = temp;
            }
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

            usedDeck.Add(deck[0]);  // 사용한 카드 묘지에 추가

            deck.RemoveAt(0);

            return cardData;
        }
        else
        {
            DeckShuffle(usedDeck);
            deck.Add(usedDeck[0]);
            usedDeck.RemoveAt(0);


            return GetCardData();
        }
    }

    public void DrawCard(bool isReroll = false)
    {
        CardData cardData = GetCardData();

        

        if(cardData != null)
        {
            GameObject cardObj = Instantiate(cardPrefab, handleTrm);
            DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();

            dragbleCard.SetDroppedArea(handleDropArea);
            dragbleCard.originDropArea = handleDropArea;

            dragbleCard.SetData_Feild(cardData.cardType, cardData.value);

            if(isReroll == false)
            {
                haveCardCount++;
                drawCount++;
            }
        }
        else
        {
            Debug.LogError("카드를 뽑을수 없다?");
        }
    }

    private void DrawBuild()
    {
        List<int> removeIdList = new List<int>();

        Dictionary<int, int> haveBuildDic = DataManager.Instance.GetHaveBuildDic();
        foreach(int id in haveBuildDic.Keys)
        {
            for(int i = 0; i < haveBuildDic[id]; i++)
            {
                GameObject buildObj = Instantiate(buildPrefab, buildHandleTrm);

                // build 관련 초기화

                Build build = buildObj.GetComponent<Build>();
                build.Init(DataManager.Instance.GetBuildSO(id));


                // dragble 관련 초기화
                DragbleCard dragbleCard = buildObj.GetComponent<DragbleCard>();

                dragbleCard.SetDroppedArea(buildHandleDropArea);
                dragbleCard.originDropArea = buildHandleDropArea;

                dragbleCard.SetData_Build();

                removeIdList.Add(id);
            }
        }

        while(removeIdList.Count != 0)
        {
            DataManager.Instance.RemoveBuild(removeIdList[0]);
            removeIdList.RemoveAt(0);
        }
    }

    private void DrawSpecialCard()
    {
        Dictionary<int, int> haveSpecialDic = DataManager.Instance.GetHaveSpecialCardDic();
        foreach (int id in haveSpecialDic.Keys)
        {
            for (int i = 0; i < haveSpecialDic[id]; i++)
            {
                GameObject specialCardObj = Instantiate(specialCardPrefab, buildHandleTrm);
                DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

                // specialCard 관련 초기화
                SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                specialCard.Init(DataManager.Instance.GetSpecialCardSO(id));

                // dragble 관련 초기화
                dragbleCard.SetDroppedArea(buildHandleDropArea);
                dragbleCard.originDropArea = buildHandleDropArea;

                dragbleCard.SetData_SpecialCard();
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
        //    Debug.Log("모든 카드 뽑음 버튼을 눌러 다음 턴으로 가세요");
        //}
        //else
        //{
        //    DrawCard();
        //}
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

        drawCount = 0;
        haveCardCount = 0;
        for (int i = 0; i < handleCount; i++)
        {
            DrawCard();
        }
    }

    public void DrawBuildAndSpecialWhenTurnStart()
    {
        Stack<DragbleCard> sellCardStack = new Stack<DragbleCard>();
        // 뽑기전에 남아있는 카드가 있다면 제거
        for (int i = 0; i < buildHandleTrm.childCount; i++)
        {
            GameObject card = buildHandleTrm.GetChild(i).gameObject;
            card.SetActive(false);
            sellCardStack.Push(card.GetComponent<DragbleCard>());
        }

        while (sellCardStack.Count != 0)
        {
            Destroy(sellCardStack.Pop().gameObject);
        }

        DrawBuild();
        DrawSpecialCard();
    }
}
