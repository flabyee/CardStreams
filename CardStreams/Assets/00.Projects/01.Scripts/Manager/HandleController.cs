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

    // plaeyr deck == playerHandleObj 
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

    [Header("system")]
    private int deckValueAmount;
    private int deckValueIncreaseAmount;
    private float deckValueIncreaseMultipication;
    private int bossDownValue;

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
        bossDownValue = stageData.downValue;
        
        maxValue = 5;

        DeckMake();
    }

    private void DeckMake()
    {
        List<CardData> allDeck = new List<CardData>();

        playerOriginDeck.Clear();
        enemyOriginDeck.Clear();

        // originDeck �����
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
            // pValue�� �� ũ�ٸ�
            if (deckValue > deckValueAmount)
            {
                // �÷��̾� ī�带 ���̰ų� �� ī�带 �ø���
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
            // mValue�� �� ũ�ٸ�
            else
            {
                // �÷��̾� ī�带 �ø��ų� �� ī�带 ���δ�
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
                enemyDeck.Add(cardData);
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


    private void AddEnemyDeck()
    {
        DeckShuffle(enemyOriginDeck);
        foreach (CardData cardData in enemyOriginDeck)
        {
            enemyDeck.Add(cardData);
        }
    }

    private IEnumerator DrawPlayerCard()
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

        //    // Ŭ�����ϰ� ���߿� moveStart�� �ٽ� �߰�
        //    playerDeck.Clear();
        //}
        //else
        //{
        //    playerDeck.Clear();

        //    AddPlayerDeck();

        //    DrawPlayerCard();
        //}

        if(playerHandleObj.Count == 0)
        {
            DeckShuffle(playerOriginDeck);

            foreach(CardData cardData in playerOriginDeck)
            {
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

                    cardSorting.AlignCards();
                });

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    private IEnumerator DrawEnemyCard()
    {
        if(enemyDeck.Count != 0)
        {
            for(int i = 0; i < 2; i++)
            {
                CardData cardData = enemyDeck[i];

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

                    cardSorting.AlignCards();
                });

                yield return new WaitForSeconds(0.2f);
            }

            enemyDeck.RemoveAt(0);
            enemyDeck.RemoveAt(0);
        }
        else
        {
            AddEnemyDeck();

            StartCoroutine(DrawEnemyCard());

            Debug.Log("add enemy deck");
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

            // build ���� �ʱ�ȭ
            BuildCard build = buildObj.GetComponent<BuildCard>();
            build.Init(DataManager.Instance.GetBuildSO(id));


            // dragble ���� �ʱ�ȭ
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
    public IEnumerator DrawSpecialCard()
    {
        Debug.Log(specialDeck.Count);
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

            BezierCard drawBezier = Instantiate(drawCardBezierEffect, drawCardStartTrm.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();
            drawBezier.Init(handleTrm, null, () =>
            {
                GameObject specialCardObj = CardPoolManager.Instance.GetSpecialCard(handleTrm);
                DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

                // specialCard ���� �ʱ�ȭ
                SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                Debug.Log(avoidClosure);
                Debug.Log("Count : " + specialDeck.Count);
                specialCard.Init(DataManager.Instance.GetSpecialCardSO(temp));

                // dragble ���� �ʱ�ȭ
                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_SpecialCard();

                specialCard.OnHandle();

                specialHandleObj.Add(specialCard);

                cardSorting.AlignCards();
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

    public void ReturnSpecialCards(List<int> idList)
    {
        foreach(int id in idList)
        {
            int temp = id;
            BezierCard drawBezier = Instantiate(drawCardBezierEffect, drawCardStartTrm.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();
            
            drawBezier.Init(handleTrm, null, () =>
            {
                GameObject specialCardObj = CardPoolManager.Instance.GetSpecialCard(handleTrm);
                DragbleCard dragbleCard = specialCardObj.GetComponent<DragbleCard>();

                // specialCard ���� �ʱ�ȭ
                SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                specialCard.Init(DataManager.Instance.GetSpecialCardSO(temp));

                // dragble ���� �ʱ�ȭ
                dragbleCard.SetDroppedArea(handleDropArea);
                dragbleCard.originDropArea = handleDropArea;

                dragbleCard.InitData_SpecialCard();

                specialCard.OnHandle();

                specialHandleObj.Add(specialCard);

                cardSorting.AlignCards();
            });
        }
    }

    // turnStart or moveEnd ���� �ϴ� draw
    public void DrawCardWhenBeforeMove()
    {
        float delay1 = playerHandleObj.Count <= 1 ? 1.2f : 0f;  // ���� ������ 6��, �ƴϸ� 0��
        float delay2 = 0.4f;    // �� ī�� 2��
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => StartCoroutine(DrawPlayerCard()));
        seq.AppendInterval(delay1); // 6��° ī�尡 1.0~1.2�ʿ� ����Ǵ� 1.2�� ���ĺ��ʹ� ��ī�� ��ο쵵 ���� ����
        seq.AppendCallback(() => StartCoroutine(DrawEnemyCard()));
        seq.AppendInterval(delay2); // 2��° ī�尡 0.2~0.4�ʿ� ����Ǵ� 0.4�� ���ĺ��ʹ� Ư��ī�� ��ο쵵 ���� ����
        seq.AppendCallback(() => StartCoroutine(DrawSpecialCard()));
    }

    public void LoopEnd()
    {
        // �����ִ� �÷��̾� �ڵ� ����
        for(int i = playerHandleObj.Count - 1; i >= 0; i--)
        {
            playerHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();
        }
        playerHandleObj.Clear();

        enemyDeck.Clear();

        deckValueAmount -= deckValueIncreaseAmount;
        maxValue = Mathf.RoundToInt((float)maxValue * deckValueIncreaseMultipication);

        deckValueIncreaseAmount = Mathf.RoundToInt((float)deckValueIncreaseAmount * deckValueIncreaseMultipication);

        deckValueAmount += bossDownValue;

        DeckMake();
    }

    // ���� �տ� �ִ� ī����� ��� �Ǹ��Ѵ�.
    public void HandleReturnToDeck()
    {
        List<BasicCard> removeIndex = new List<BasicCard>();
        for (int i = 0; i < playerHandleObj.Count; i++)
        {
            // �տ� �ִٸ� active false
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
        // ���� ������ ������
        if (playerHandleObj.Count == 1)
        {
            playerHandleObj[0].GetComponent<DragbleCard>().ActiveFalse();
            playerHandleObj.Clear();
        }

        for (int i = 0; i < enemyHandleObj.Count; i++)
        {
            // �տ� �ִٸ� active false

            int iAvoidClosure = i;

            if (enemyHandleObj[i].isHandle == true)
            {
                enemyHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                // �����ε��ư��⿬��
                BezierCard returnBezier = Instantiate(drawCardBezierEffect, enemyHandleObj[i].transform.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();

                returnBezier.Init(drawCardStartTrm, null, null);
            }
        }
        enemyHandleObj.Clear();

        for (int i = 0; i < buildHandleObj.Count; i++)
        {
            // �տ� �ִٸ� active false
            if (buildHandleObj[i].isHandle == true)
            {
                buildDeck.Add(buildHandleObj[i].buildSO.id);
                buildHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                // �����ε��ư��⿬��
                BezierCard returnBezier = Instantiate(drawCardBezierEffect, buildHandleObj[i].transform.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();

                returnBezier.Init(drawCardStartTrm, null, null);
            }
        }
        buildHandleObj.Clear();

        for (int i = 0; i < specialHandleObj.Count; i++)
        {
            // �տ� �ִٸ� active false
            if (specialHandleObj[i].isHandle == true)
            {
                specialDeck.Add(specialHandleObj[i].id);
                specialHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                // �����ε��ư��⿬��
                BezierCard returnBezier = Instantiate(drawCardBezierEffect, specialHandleObj[i].transform.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();

                returnBezier.Init(drawCardStartTrm, null, null);
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

    public List<int> GetBuildDeck()
    {
        return buildDeck;
    }
    public List<int> GetSpecialDeck()
    {
        return specialDeck;
    }
}