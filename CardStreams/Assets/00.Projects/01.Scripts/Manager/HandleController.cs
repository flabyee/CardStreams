using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class HandleController : MonoBehaviour
{
    private List<CardData> playerOriginDeck = new List<CardData>();
    private List<CardData> enemyOriginDeck = new List<CardData>();

    private List<CardData> playerDeck = new List<CardData>();
    private List<CardData> enemyDeck = new List<CardData>();

    private List<int> buildDeck = new List<int>();
    private List<int> specialDeck = new List<int>();

    private List<BasicCard> playerHandleObj = new List<BasicCard>();
    private List<BasicCard> enemyHandleObj = new List<BasicCard>();
    private List<BuildCard> buildHandleObj = new List<BuildCard>();
    private List<SpecialCard> specialHandleObj = new List<SpecialCard>();

    [Header("UI")]
    public DropArea handleDropArea;
    public RectTransform handleTrm;

    [Header("system")]
    private int deckValueAmount;
    private int deckValueIncreaseAmount;
    private float deckValueIncreaseMultipication;

    private float originHandleY;
    public float handleMoveAmount;

    public CardSorting cardSorting;

    private int maxValue;

    private void Awake()
    {
        originHandleY = handleTrm.anchoredPosition.y;

        cardSorting = handleTrm.GetComponent<CardSorting>();
    }

    private void Start()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();

        deckValueAmount = stageData.firstDeckValueAmount;
        deckValueIncreaseAmount = stageData.deckValueIncreaseAmount;
        deckValueIncreaseMultipication = stageData.deckValueIncreaseMultipication;
        maxValue = 5;

        DeckMake();
    }

    private void DeckMake()
    {
        List<CardData> allDeck = new List<CardData>();

        playerOriginDeck.Clear();
        enemyOriginDeck.Clear();

        // originDeck 만들고
        for (int i = 0; i < 12; i++)
        {
            switch (i % 12)
            {
                case 0:
                case 1:
                    allDeck.Add(new CardData(BasicType.Sword, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 2:
                case 3:
                    allDeck.Add(new CardData(BasicType.Sheild, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 4:
                case 5:
                    allDeck.Add(new CardData(BasicType.Potion, UnityEngine.Random.Range(2, maxValue)));
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    allDeck.Add(new CardData(BasicType.Monster, UnityEngine.Random.Range(2, 5)));
                    break;
            }
        }

        int pValue = 0;
        int mValue = 0;

        foreach (CardData cardData in allDeck)
        {
            if (cardData.basicType != BasicType.Monster)
                pValue += cardData.value;
            else
                mValue += cardData.value;
        }

        int deckValue = pValue - mValue;
        int randomIndex = 0;

        while (deckValue != deckValueAmount)
        {
            randomIndex = UnityEngine.Random.Range(0, 12);
            // pValue가 더 크다면
            if (deckValue > deckValueAmount)
            {
                // 플레이어 카드를 줄이거나 몹 카드를 늘린다
                if (allDeck[randomIndex].basicType != BasicType.Monster)
                {
                    if (allDeck[randomIndex].value > 1)
                        allDeck[randomIndex].value--;
                    else
                        deckValue++;
                }
                else
                {
                    if (allDeck[randomIndex].value < maxValue)
                        allDeck[randomIndex].value++;
                    else
                        deckValue++;
                }

                deckValue--;
            }
            // mValue가 더 크다면
            else
            {
                // 플레이어 카드를 늘리거나 몹 카드를 줄인다
                if (allDeck[randomIndex].basicType != BasicType.Monster)
                {
                    if (allDeck[randomIndex].value < maxValue)
                        allDeck[randomIndex].value++;
                    else
                        deckValue--;
                }
                else
                {
                    if (allDeck[randomIndex].value > 1)
                        allDeck[randomIndex].value--;
                    else
                        deckValue--;
                }


                deckValue++;
            }
        }

        foreach (CardData cardData in allDeck)
        {
            if (cardData.basicType != BasicType.Monster)
            {
                playerOriginDeck.Add(cardData);
            }
        }

        DeckShuffle(allDeck);

        foreach (CardData cardData in allDeck)
        {
            if (cardData.basicType == BasicType.Monster)
            {
                enemyOriginDeck.Add(cardData);
            }
        }
    }

    private void DeckShuffle(List<CardData> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            int j = UnityEngine.Random.Range(0, cardList.Count);
            CardData temp = cardList[i];
            cardList[i] = cardList[j];
            cardList[j] = temp;
        }
    }

    private void AddPlayerDeck()
    {
        foreach (CardData cardData in playerOriginDeck)
        {
            playerDeck.Add(cardData);
        }
    }
    private void AddEnemyDeck()
    {
        DeckMake();

        DeckShuffle(enemyOriginDeck);
        foreach (CardData cardData in enemyOriginDeck)
        {
            enemyDeck.Add(cardData);
        }
    }

    private void DrawPlayerCard()
    {
        //if(playerDeck.Count > 1)
        //{
        //    foreach(CardData cardData in playerDeck)
        //    {
        //        GameObject cardObj = CardPoolManager.Instance.GetBasicCard(handleTrm);
        //        DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
        //        BasicCard basicCard = cardObj.GetComponent<BasicCard>();

        //        dragbleCard.SetDroppedArea(handleDropArea);
        //        dragbleCard.originDropArea = handleDropArea;

        //        dragbleCard.InitData_Feild(cardData.basicType, cardData.value);

        //        basicCard.OnHandle();

        //        playerHandleObj.Add(basicCard);
        //    }

        //    // 클리어하고 나중에 moveStart때 다시 추가
        //    playerDeck.Clear();
        //}
        //else
        //{
        //    playerDeck.Clear();

        //    AddPlayerDeck();

        //    DrawPlayerCard();
        //}

        if(playerHandleObj.Count <= 1)
        {
            foreach(BasicCard card in playerHandleObj)
            {
                card.GetComponent<DragbleCard>().ActiveFalse();
            }
            playerHandleObj.Clear();

            DeckShuffle(playerOriginDeck);

            foreach(CardData cardData in playerOriginDeck)
            {
                GameObject cardObj = CardPoolManager.Instance.GetBasicCard(handleTrm);
                DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
                BasicCard basicCard = cardObj.GetComponent<BasicCard>();

                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_Feild(cardData.basicType, cardData.value);

                basicCard.OnHandle();

                playerHandleObj.Add(basicCard);
            }
        }

        cardSorting.AlignCards();
    }
    private void DrawEnemyCard()
    {
        if(enemyDeck.Count != 0)
        {
            for(int i = 0; i < 2; i++)
            {
                CardData cardData = enemyDeck[i];

                GameObject cardObj = CardPoolManager.Instance.GetBasicCard(handleTrm);
                DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
                BasicCard basicCard = cardObj.GetComponent<BasicCard>();

                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_Feild(cardData.basicType, cardData.value);

                basicCard.OnHandle();

                enemyHandleObj.Add(basicCard);
            }

            enemyDeck.RemoveAt(0);
            enemyDeck.RemoveAt(0);
        }
        else
        {
            AddEnemyDeck();

            DrawEnemyCard();
        }

    }

    public void AddBuild(int id)
    {
        buildDeck.Add(id);
    }
    public void AddSpecial(int id)
    {
        specialDeck.Add(id);
    }

    public void DrawBuildCard()
    {
        foreach(int id in buildDeck)
        {
            GameObject buildObj = CardPoolManager.Instance.GetBuildCard(handleTrm);

            // build 관련 초기화
            BuildCard build = buildObj.GetComponent<BuildCard>();
            build.Init(DataManager.Instance.GetBuildSO(id));


            // dragble 관련 초기화
            DragbleCard dragbleCard = buildObj.GetComponent<DragbleCard>();

            dragbleCard.SetDroppedArea(handleDropArea);
            dragbleCard.originDropArea = handleDropArea;

            dragbleCard.InitData_Build();

            build.OnHandle();

            buildHandleObj.Add(build);
        }

        buildDeck.Clear();

        cardSorting.AlignCards();
    }
    public void DrawSpecialCard()
    {
        for (int i = 0; i < specialDeck.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, specialDeck.Count);
            int temp = specialDeck[i];
            specialDeck[i] = specialDeck[randomIndex];
            specialDeck[randomIndex] = temp;
        }

        for (int i = 0; i < specialDeck.Count; i++)
        {
            if (i == 2)
                break;

            GameObject specialCardObj = CardPoolManager.Instance.GetSpecialCard(handleTrm);
            DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

            // specialCard 관련 초기화
            SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
            specialCard.Init(DataManager.Instance.GetSpecialCardSO(specialDeck[i]));

            // dragble 관련 초기화
            dragbleCard.SetDroppedArea(handleDropArea);
            dragbleCard.originDropArea = handleDropArea;

            dragbleCard.InitData_SpecialCard();

            specialCard.OnHandle();

            specialHandleObj.Add(specialCard);
        }

        for (int i = 0; i < specialDeck.Count; i++)
        {
            if (i == 2)
                break;

            specialDeck.RemoveAt(0);
        }

    }

    // turnStart or moveEnd 마다 하는 draw
    public void DrawCardWhenBeforeMove()
    {
        DrawPlayerCard();
        DrawEnemyCard();
        DrawSpecialCard();

        cardSorting.AlignCards();
    }

    public void LoopEnd()
    {
        playerDeck.Clear();
        enemyDeck.Clear();

        deckValueAmount -= deckValueIncreaseAmount;
        maxValue = Mathf.RoundToInt((float)maxValue * deckValueIncreaseMultipication);

        deckValueIncreaseAmount = Mathf.RoundToInt((float)deckValueIncreaseAmount * deckValueIncreaseMultipication);

        DeckMake();
    }

    // 현재 손에 있는 카드들을 모두 판매한다.
    public void HandleReturnToDeck()
    {
        List<BasicCard> removeIndex = new List<BasicCard>();
        for (int i = 0; i < playerHandleObj.Count; i++)
        {
            // 손에 있다면 active false
            if (playerHandleObj[i].isField == true)
            {
                removeIndex.Add(playerHandleObj[i]);
            }
        }
        while(removeIndex.Count != 0)
        {
            playerHandleObj.Remove(removeIndex[0]);
            removeIndex.RemoveAt(0);
        }

        for (int i = 0; i < enemyHandleObj.Count; i++)
        {
            // 손에 있다면 active false
            if (enemyHandleObj[i].isHandle == true)
            {
                enemyHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();
            }
        }
        enemyHandleObj.Clear();

        for (int i = 0; i < buildHandleObj.Count; i++)
        {
            // 손에 있다면 active false
            if (buildHandleObj[i].isHandle == true)
            {
                buildDeck.Add(buildHandleObj[i].buildSO.id);

                buildHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();
            }
        }
        buildHandleObj.Clear();

        for (int i = 0; i < specialHandleObj.Count; i++)
        {
            // 손에 있다면 active false
            if (specialHandleObj[i].isHandle == true)
            {
                specialDeck.Add(specialHandleObj[i].id);

                specialHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();
            }
        }
        specialHandleObj.Clear();

        cardSorting.AlignCards();
    }

    public void MoveHandle(bool isDown)
    {
        if(isDown == true)
        {
            handleTrm.DOAnchorPosY(originHandleY + handleMoveAmount, 0.5f);
        }
        else
        {
            handleTrm.DOAnchorPosY(originHandleY, 0.5f);
        }
        
    }
}