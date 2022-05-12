using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandleManager : MonoBehaviour
{
    [HideInInspector] public List<CardData> originDeck = new List<CardData>();
    [HideInInspector] public List<CardData> deck = new List<CardData>();
    [HideInInspector] public List<CardData> usedDeck = new List<CardData>();    // ����

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

    private void Awake()
    {
        
    }

    private void Start()
    {
        originDeck = DataManager.Instance.GetNowStageData().deck;

        handleCount = DataManager.Instance.GetNowStageData().moveCount;

        isDeckShuffle = DataManager.Instance.GetNowStageData().isDeckShuffle;

        DeckMake(originDeck);
    }

    private void AddAllCardData(CardType cardType, string str)
    {
        string[] strs = str.Split(' ');
        foreach (string item in strs)
        {
            deck.Add(new CardData(cardType, int.Parse(item), DropAreaType.feild));
        }
    }
    private void DeckMake(List<CardData> originDeck)
    {
        //AddAllCardData(CardType.Sword, "2 3 4");
        //AddAllCardData(CardType.Sheild, "3 4 5");
        //AddAllCardData(CardType.Monster, "3 3 3 3 4 4 4");
        //AddAllCardData(CardType.Potion, "2 3 3 4 4");
        ////AddAllCardData(CardType.Coin, "0 0 0 0 0 0");

        deck.Clear();

        foreach(CardData cardData in originDeck)
        {
            deck.Add(cardData);
        }

        if(isDeckShuffle == true)
        {
            DeckShuffle(deck);
        }
    }

    public void CardRerollAdd(GameObject dragbleCardObj)
    {
        CardPower cardPower = dragbleCardObj.GetComponent<CardPower>();

        deck.Add(new CardData(cardPower.cardType, cardPower.value, cardPower.dropAreaType));

        DeckShuffle(deck, true);
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
            DeckMake(originDeck);

            return GetCardData();
        }
    }

    public void DrawCard()
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

        for (int i = 0; i < handleCount; i++)
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
}
