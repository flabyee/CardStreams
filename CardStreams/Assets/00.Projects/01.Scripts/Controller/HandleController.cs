using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class HandleController : MonoBehaviour
{
    private List<CardData> playerOriginDeck = new List<CardData>();     // 앞에 12칸 이동 때 사용
    private List<CardData> playerOriginDeck2 = new List<CardData>();    // 뒤에 12칸 이동 때 사용
    private List<CardData> enemyOriginDeck = new List<CardData>();

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
    public Transform drawCardStartTrm;
    public GameObject drawCardBezierEffect;
    public GameObject notHaveBuildUI = null;

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

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        cardSorting.AlignCards();
    //    }
    //}

    // origin decks, enemyDeck 초기화
    private void DeckMake()
    {
        List<CardData> allDeck = new List<CardData>();

        playerOriginDeck.Clear();
        enemyOriginDeck.Clear();
        playerOriginDeck2.Clear();

        // allDeck 만들고
        {
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
        }

        // origin deck에 추가
        {
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


        // allDeck2 만들고
        {
            allDeck.Clear();
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
        }

        // origin deck2 에 추가
        {
            foreach (CardData cardData in allDeck)
            {
                if (cardData.basicType != BasicType.Monster)
                {
                    playerOriginDeck2.Add(cardData);
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

        AddEnemyDeck();
    }

    // 범용적으로 사용할 수 있는 덱 셔플 함수
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


    #region 플레이어 카드 관련
    private IEnumerator DrawPlayerCard()
    {
        foreach (CardData cardData in playerOriginDeck)
        {
            SoundManager.Instance.PlaySFX(SFXType.DrawCard);
            BezierCard drawBezier = Instantiate(drawCardBezierEffect, drawCardStartTrm.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();
            drawBezier.Init(handleTrm, null, () =>
            {
                GameObject cardObj = CardPoolManager.Instance.GetBasicCard(handleTrm);
                DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
                BasicCard basicCard = cardObj.GetComponent<BasicCard>();

                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_Feild(cardData.basicType, cardData.value);

                basicCard.OnHandle();

                playerHandleObj.Add(basicCard);

                cardSorting.AddList(basicCard);
            });

            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator DrawPlayerCard2()
    {
        foreach (CardData cardData in playerOriginDeck2)
        {
            SoundManager.Instance.PlaySFX(SFXType.DrawCard);
            BezierCard drawBezier = Instantiate(drawCardBezierEffect, drawCardStartTrm.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();
            drawBezier.Init(handleTrm, null, () =>
            {
                GameObject cardObj = CardPoolManager.Instance.GetBasicCard(handleTrm);
                DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
                BasicCard basicCard = cardObj.GetComponent<BasicCard>();

                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_Feild(cardData.basicType, cardData.value);

                basicCard.OnHandle();

                playerHandleObj.Add(basicCard);

                cardSorting.AddList(basicCard);
            });

            yield return new WaitForSeconds(0.2f);
        }
    }
    // 가지고 있던 플레이어 카드 버리는 함수, 다른 카드는 움직일 때 마다 버려주기 때문에 플레이어만 따로 있다
    private void ClearHavingPlayerCard()
    {
        for (int i = 0; i < playerHandleObj.Count; i++)
        {
            if (playerHandleObj[i].isHandle == true)
            {
                playerHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                cardSorting.RemoveList(playerHandleObj[i]);
            }
        }
        playerHandleObj.Clear();
    }
    #endregion

    #region 몬스터 카드 관련
    private IEnumerator DrawEnemyCard()
    {
        if(enemyDeck.Count > 3)
        {
            for(int i = 0; i < 4; i++)
            {
                CardData cardData = enemyDeck[i];
                SoundManager.Instance.PlaySFX(SFXType.DrawCard);
                BezierCard drawBezier = Instantiate(drawCardBezierEffect, drawCardStartTrm.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();
                drawBezier.Init(handleTrm, null, () =>
                {
                    GameObject cardObj = CardPoolManager.Instance.GetBasicCard(handleTrm);
                    DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
                    BasicCard basicCard = cardObj.GetComponent<BasicCard>();

                    dragbleCard.SetDroppedArea(handleDropArea);
                    dragbleCard.originDropArea = handleDropArea;

                    dragbleCard.InitData_Feild(cardData.basicType, cardData.value);

                    basicCard.OnHandle();

                    enemyHandleObj.Add(basicCard);

                    cardSorting.AddList(basicCard);
                });

                yield return new WaitForSeconds(0.2f);
            }

            enemyDeck.RemoveAt(0);
            enemyDeck.RemoveAt(0);
            enemyDeck.RemoveAt(0);
            enemyDeck.RemoveAt(0);
        }
        else
        {
            if(enemyDeck.Count != 0)
            {
                Debug.LogError("12장에서 4장씩 빼갔는데 4장 단위로 딱 떨어지지 않음");
                enemyDeck.Clear();
            }

            // 12장 다쓰면 다시 12장 보충
            AddEnemyDeck();
            StartCoroutine(DrawEnemyCard());
        }

    }
    private void AddEnemyDeck()
    {
        Debug.Log(enemyOriginDeck.Count + "deck count");
        DeckShuffle(enemyOriginDeck);
        foreach (CardData cardData in enemyOriginDeck)
        {
            enemyDeck.Add(cardData);
        }
    }
    #endregion

    #region 특수, 건물 카드 관련
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
        if(buildDeck.Count == 0)
        {
            notHaveBuildUI.SetActive(true);
            return;
        }

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

            cardSorting.AddList(build);
        }

        buildDeck.Clear();
    }
    public IEnumerator DrawSpecialCard()
    {
        //Debug.Log(specialDeck.Count);
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

            int avoidClosure = i;
            int temp = specialDeck[avoidClosure];

            SoundManager.Instance.PlaySFX(SFXType.DrawCard);
            BezierCard drawBezier = Instantiate(drawCardBezierEffect, drawCardStartTrm.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();
            drawBezier.Init(handleTrm, null, () =>
            {
                GameObject specialCardObj = CardPoolManager.Instance.GetSpecialCard(handleTrm);
                DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

                // specialCard 관련 초기화
                SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                //Debug.Log(avoidClosure);
                //Debug.Log("Count : " + specialDeck.Count);
                specialCard.Init(DataManager.Instance.GetSpecialCardSO(temp));

                // dragble 관련 초기화
                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_SpecialCard();

                specialCard.OnHandle();

                specialHandleObj.Add(specialCard);

                cardSorting.AddList(specialCard);
            });

            yield return new WaitForSeconds(0.2f);
        }

        for (int i = 0; i < specialDeck.Count; i++)
        {
            if (i == 2)
                break;

            specialDeck.RemoveAt(0);
        }

    }
    // 필드에 사용한 특수카드를 다시 회수할 때 사용하는 함수
    public void ReturnSpecialCards(List<int> idList)
    {
        foreach(int id in idList)
        {
            int temp = id;
            SoundManager.Instance.PlaySFX(SFXType.DrawCard);
            BezierCard drawBezier = Instantiate(drawCardBezierEffect, drawCardStartTrm.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();
            
            drawBezier.Init(handleTrm, null, () =>
            {
                GameObject specialCardObj = CardPoolManager.Instance.GetSpecialCard(handleTrm);
                DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

                // specialCard 관련 초기화
                SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                specialCard.Init(DataManager.Instance.GetSpecialCardSO(temp));

                // dragble 관련 초기화
                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_SpecialCard();

                specialCard.OnHandle();

                specialHandleObj.Add(specialCard);

                cardSorting.AddList(specialCard);
            });

            MissionObserverManager.instance.OffSpecialCard?.Invoke(id);
        }
    }
    public List<int> GetBuildDeck()
    {
        return buildDeck;
    }
    public List<int> GetSpecialDeck()
    {
        return specialDeck;
    }
    #endregion

    // turnStart or moveEnd 마다 하는 draw
    public void DrawCardWhenBeforeMove()
    {
        float playerDrawDelay = 1.2f;    // 플레이어 카드 4장
        float mobDrawDelay = 0.8f;    // 적 카드 4장

        Sequence seq = DOTween.Sequence();

        if(GameManager.Instance.moveIndex == 0)
        {
            seq.AppendCallback(() => StartCoroutine(DrawPlayerCard()));
            seq.AppendInterval(playerDrawDelay);
        }
        if(GameManager.Instance.moveIndex == 12)
        {
            seq.AppendCallback(() => StartCoroutine(DrawPlayerCard2()));
            seq.AppendInterval(playerDrawDelay);
        }

        seq.AppendCallback(() => StartCoroutine(DrawEnemyCard()));
        seq.AppendInterval(mobDrawDelay);
        seq.AppendCallback(() => StartCoroutine(DrawSpecialCard()));
    }

    // 몬스터, 건물, 특수 카드 버리는 함수, 특수, 건물카드는 다시 덱으로 돌아간다
    public void HandleReturnToDeck()
    {
        // 몬스터카드 버리기
        for (int i = 0; i < enemyHandleObj.Count; i++)
        {
            if (enemyHandleObj[i].isHandle == true)
            {
                enemyHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                cardSorting.RemoveList(enemyHandleObj[i]);
            }
        }
        enemyHandleObj.Clear();

        // 건물카드 버리기
        for (int i = 0; i < buildHandleObj.Count; i++)
        {
            // 손에 있다면 active false
            if (buildHandleObj[i].isHandle == true)
            {
                buildDeck.Add(buildHandleObj[i].buildSO.id);
                buildHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                // 덱으로돌아가기연출
                SoundManager.Instance.PlaySFX(SFXType.DrawCard);
                BezierCard returnBezier = Instantiate(drawCardBezierEffect, buildHandleObj[i].transform.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();

                returnBezier.Init(drawCardStartTrm, null, null);

                cardSorting.RemoveList(buildHandleObj[i]);
            }
        }
        buildHandleObj.Clear();

        // 특수카드 버리기
        for (int i = 0; i < specialHandleObj.Count; i++)
        {
            // 손에 있다면 active false
            if (specialHandleObj[i].isHandle == true)
            {
                specialDeck.Add(specialHandleObj[i].id);
                specialHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                // 덱으로돌아가기연출
                SoundManager.Instance.PlaySFX(SFXType.DrawCard);
                BezierCard returnBezier = Instantiate(drawCardBezierEffect, specialHandleObj[i].transform.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();

                returnBezier.Init(drawCardStartTrm, null, null);

                cardSorting.RemoveList(specialHandleObj[i]);
            }
        }
        specialHandleObj.Clear();

        if(GameManager.Instance.moveIndex == 12)
            ClearHavingPlayerCard();

        // 버릴 것 버린 이후에 다시 카드 정렬하기
        cardSorting.AlignCards();
    }

    // 루프 종료 이후 덱에서 해야할 처리하는 함수(기존에 패 초기화 및 덱 새로 생성)
    public void LoopEnd()
    {
        // 남아있는 플레이어 핸드 제거
        for(int i = playerHandleObj.Count - 1; i >= 0; i--)
        {
            if(playerHandleObj[i].isField == false)
            {
                playerHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();
                cardSorting.RemoveList(playerHandleObj[i]);
            }
        }
        playerHandleObj.Clear();

        enemyDeck.Clear();

        deckValueAmount -= deckValueIncreaseAmount;
        maxValue = Mathf.RoundToInt((float)maxValue * deckValueIncreaseMultipication);

        deckValueIncreaseAmount = Mathf.RoundToInt((float)deckValueIncreaseAmount * deckValueIncreaseMultipication);

        DeckMake();
    }

    // 카드를 들면 패가 밑으로 내려가서 더 넓게 보이게 해주는 함수
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