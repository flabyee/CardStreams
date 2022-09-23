using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class HandleController : MonoBehaviour
{
    private List<CardData> playerOriginDeck = new List<CardData>();     // �տ� 12ĭ �̵� �� ���
    private List<CardData> playerOriginDeck2 = new List<CardData>();    // �ڿ� 12ĭ �̵� �� ���
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

    // origin decks, enemyDeck �ʱ�ȭ
    private void DeckMake()
    {
        List<CardData> allDeck = new List<CardData>();

        playerOriginDeck.Clear();
        enemyOriginDeck.Clear();
        playerOriginDeck2.Clear();

        // allDeck �����
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
        }

        // origin deck�� �߰�
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


        // allDeck2 �����
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
        }

        // origin deck2 �� �߰�
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

    // ���������� ����� �� �ִ� �� ���� �Լ�
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


    #region �÷��̾� ī�� ����
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
    // ������ �ִ� �÷��̾� ī�� ������ �Լ�, �ٸ� ī��� ������ �� ���� �����ֱ� ������ �÷��̾ ���� �ִ�
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

    #region ���� ī�� ����
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
                Debug.LogError("12�忡�� 4�徿 �����µ� 4�� ������ �� �������� ����");
                enemyDeck.Clear();
            }

            // 12�� �پ��� �ٽ� 12�� ����
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

    #region Ư��, �ǹ� ī�� ����
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

                // specialCard ���� �ʱ�ȭ
                SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                //Debug.Log(avoidClosure);
                //Debug.Log("Count : " + specialDeck.Count);
                specialCard.Init(DataManager.Instance.GetSpecialCardSO(temp));

                // dragble ���� �ʱ�ȭ
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
    // �ʵ忡 ����� Ư��ī�带 �ٽ� ȸ���� �� ����ϴ� �Լ�
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

                // specialCard ���� �ʱ�ȭ
                SpecialCard specialCard = specialCardObj.GetComponent<SpecialCard>();
                specialCard.Init(DataManager.Instance.GetSpecialCardSO(temp));

                // dragble ���� �ʱ�ȭ
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

    // turnStart or moveEnd ���� �ϴ� draw
    public void DrawCardWhenBeforeMove()
    {
        float playerDrawDelay = 1.2f;    // �÷��̾� ī�� 4��
        float mobDrawDelay = 0.8f;    // �� ī�� 4��

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

    // ����, �ǹ�, Ư�� ī�� ������ �Լ�, Ư��, �ǹ�ī��� �ٽ� ������ ���ư���
    public void HandleReturnToDeck()
    {
        // ����ī�� ������
        for (int i = 0; i < enemyHandleObj.Count; i++)
        {
            if (enemyHandleObj[i].isHandle == true)
            {
                enemyHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                cardSorting.RemoveList(enemyHandleObj[i]);
            }
        }
        enemyHandleObj.Clear();

        // �ǹ�ī�� ������
        for (int i = 0; i < buildHandleObj.Count; i++)
        {
            // �տ� �ִٸ� active false
            if (buildHandleObj[i].isHandle == true)
            {
                buildDeck.Add(buildHandleObj[i].buildSO.id);
                buildHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                // �����ε��ư��⿬��
                SoundManager.Instance.PlaySFX(SFXType.DrawCard);
                BezierCard returnBezier = Instantiate(drawCardBezierEffect, buildHandleObj[i].transform.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();

                returnBezier.Init(drawCardStartTrm, null, null);

                cardSorting.RemoveList(buildHandleObj[i]);
            }
        }
        buildHandleObj.Clear();

        // Ư��ī�� ������
        for (int i = 0; i < specialHandleObj.Count; i++)
        {
            // �տ� �ִٸ� active false
            if (specialHandleObj[i].isHandle == true)
            {
                specialDeck.Add(specialHandleObj[i].id);
                specialHandleObj[i].GetComponent<DragbleCard>().ActiveFalse();

                // �����ε��ư��⿬��
                SoundManager.Instance.PlaySFX(SFXType.DrawCard);
                BezierCard returnBezier = Instantiate(drawCardBezierEffect, specialHandleObj[i].transform.position, Quaternion.identity, handleTrm.parent).GetComponent<BezierCard>();

                returnBezier.Init(drawCardStartTrm, null, null);

                cardSorting.RemoveList(specialHandleObj[i]);
            }
        }
        specialHandleObj.Clear();

        if(GameManager.Instance.moveIndex == 12)
            ClearHavingPlayerCard();

        // ���� �� ���� ���Ŀ� �ٽ� ī�� �����ϱ�
        cardSorting.AlignCards();
    }

    // ���� ���� ���� ������ �ؾ��� ó���ϴ� �Լ�(������ �� �ʱ�ȭ �� �� ���� ����)
    public void LoopEnd()
    {
        // �����ִ� �÷��̾� �ڵ� ����
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

    // ī�带 ��� �а� ������ �������� �� �а� ���̰� ���ִ� �Լ�
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