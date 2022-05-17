using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public enum CardType
{
    NULL,
    Sword,
    Sheild,
    Potion,
    Monster,
    Coin,
    Special,
    Build,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("System")]
    private bool isMoving;  // move���϶� �� next�� ������ ���ϰ�
    [SerializeField] float moveDuration;
    [SerializeField] float lastDuration;

    [HideInInspector]public int rerollCount;

    public bool canStartTurn;

    [Header("UI")]
    public Player player;
    public GameObject cardPrefab;

    // stageData
    private int maxMoveCount = 3;  // n
    private int moveIndex = 0;
    private int moveCount = 0;  // n���� �����ϰŴ�
    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;  // �� ������ ������
    private int mobAttackAmount;          // �� ���ݷ�
    private int mobAttackIncreaseAmount;    // �� ���ݷ� ������



    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue turnCountValue;
    

    [Header("Event")]
    public EventSO GameStartEvent;
    public EventSO TurnStartEvent;
    public EventSO TurnEndEvent;
    public EventSO FieldResetAfter;
    public EventSO MoveStartEvent;
    public EventSO MoveEndEvent;

    public EventSO goldChangeEvent;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();
        maxMoveCount = stageData.moveCount;
        mobSpawnAmount = stageData.firstMobSpawnAmount;
        mobSpawnIncreaseAmount = stageData.mobIncreaseAmount;
        mobAttackAmount = stageData.firstMobAttackAmount;
        mobAttackIncreaseAmount = stageData.mobAttackIncreaseAmount;

        GameStartEvent.Occurred();

        goldValue.RuntimeValue += 20;
        goldChangeEvent.Occurred();

        //TurnStart();
    }

    private void Update()
    {
        
    }



    public void AddScore(int amount)
    {
        goldValue.RuntimeValue += amount;

        goldChangeEvent.Occurred();
    }

    public void RerollScore()
    {
        goldValue.RuntimeValue -= 1 + 1 * rerollCount;

        goldChangeEvent.Occurred();

        rerollCount++;
    }

    //public void OnClickDeckSee()
    //{
    //    isDeckSee = !isDeckSee;
    //    deckSeePanel.gameObject.SetActive(isDeckSee);
    //    player.gameObject.SetActive(!isDeckSee);

    //    if (isDeckSee == true)
    //    {
    //        foreach (RectTransform item in deckSeePanelTrm)
    //        {
    //            Destroy(item.gameObject);
    //        }

    //        monsterInt = 0;
    //        hpInt = 0;

    //        foreach (var item in HandleManager.Instance.deck)
    //        {
    //            Instantiate(seeCardPrefab, deckSeePanelTrm);
    //            CardPower cardPower = seeCardPrefab.GetComponent<CardPower>();
    //            cardPower.cardType = item.cardType;
    //            cardPower.value = item.value;
    //            cardPower.ApplyUI();

                

    //            if (item.cardType == CardType.Monster)
    //            {
    //                monsterInt += item.value;
    //            }

    //            if (item.cardType == CardType.Potion 
    //                || item.cardType == CardType.Sheild 
    //                || item.cardType == CardType.Sword)
    //            {
    //                hpInt += item.value;
    //            }
    //        }

    //        hpIntText.text = $"hp : {hpInt}";
    //        mobIntText.text = $"mob : {monsterInt}";
    //    }
    //}

    public void SetPlayerPos()
    {
        StartCoroutine(TempPlayerPosSetCor());
    }

    private IEnumerator TempPlayerPosSetCor()
    {
        yield return new WaitForSeconds(0.25f);

        Vector3 movePos = MapManager.Instance.fieldList[MapManager.Instance.fieldList.Count - 1].transform.position;
        player.transform.DOMove(movePos, 0.25f);
    }


    public void TurnStart()
    {
        if (moveIndex == 0)
        {
            // �� ����
            turnCountValue.RuntimeValue++;

            // ��� �ʵ��� �ʵ�Ÿ�� yet����
            foreach (Field field in MapManager.Instance.fieldList)
            {
                field.fieldType = FieldType.yet;
            }


            // ���� �� ����
            CreateRandomMob();

            // �տ� 3ĭ Ȱ��ȭ
            for (int i = 0; i < maxMoveCount; i++) 
            {
                if(MapManager.Instance.fieldList[i].fieldType == FieldType.yet)
                {
                    MapManager.Instance.fieldList[i].fieldType = FieldType.able;

                    MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.nowFieldSprite;
                }
            }

            // change mode Ȱ��ȭ
            //isChange = true;

            // ��ġ ����
            mobSpawnAmount += mobSpawnIncreaseAmount;
            mobAttackAmount += mobAttackIncreaseAmount;

            // ī�� �̱�
            TurnStartEvent.Occurred();
        }
    }

    private void CreateRandomMob()
    {
        // �Ͻ��۽� ����? ��ġ�Ұ�Ÿ��? ����
        bool[] isMonster = new bool[30];
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            if (i < mobSpawnAmount)
                isMonster[i] = true;
            else
                isMonster[i] = false;
        }
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            int j = UnityEngine.Random.Range(0, MapManager.Instance.fieldList.Count);
            bool temp = isMonster[i];
            isMonster[i] = isMonster[j];
            isMonster[j] = temp;
        }
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            if (isMonster[i] == true)
            {
                int value = mobAttackAmount; // �����Ǵ� ������ ��

                // ���ο� ī�� ����
                GameObject cardObj = Instantiate(cardPrefab, MapManager.Instance.fieldList[i].transform);
                DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
                CardPower cardPower = cardObj.GetComponent<CardPower>();

                // cardPower�� ���� �ֱ�
                dragbleCard.SetData_Feild(CardType.Monster, value);

                // �� �����̰�
                dragbleCard.canDragAndDrop = false;

                // �ʵ忡 ���� + not����
                MapManager.Instance.fieldList[i].cardPower = cardPower;
                MapManager.Instance.fieldList[i].dragbleCard = dragbleCard;
                MapManager.Instance.fieldList[i].fieldType = FieldType.not;

                // ���� ����
                cardPower.backImage.color = Color.magenta;

                // craete effect
                EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[i].transform.position);
            }
        }
    }

    public void TurnEnd()
    {
        // ���� 4���� �ʵ�
        for (int i = moveIndex - 4; i < moveIndex; i++)
        {
            // (fieldType = not)
            MapManager.Instance.fieldList[i].fieldType = FieldType.not;

            // drag and drop ���ϰ�
            MapManager.Instance.fieldList[i].dragbleCard.canDragAndDrop = false;

            MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.originFieldSprite;
        }

        moveIndex = 0;
        moveCount = 0;
        isMoving = false;


        // ����
        StartCoroutine(JungSanCor());

        // NextTurnEvent���� TurnStart�� ���ش�
    }

    public IEnumerator JungSanCor()
    {
        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            Field nowField = MapManager.Instance.fieldList[i];

            CardPower cardPower = nowField.cardPower;
            if (cardPower.cardType == CardType.Monster)
            {
                // �ʵ� ����
                MapManager.Instance.fieldList[i].FieldReset();

                // effect
                EffectManager.Instance.GetJungSanEffect(nowField.transform.position);

                // coin ����
                GoldAnimManager.Instance.CreateCoin(cardPower.value, nowField.transform.position);

                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                // �ʵ� ����
                nowField.FieldReset();
            }
        }

        yield return new WaitForSeconds(1f);

        GoldAnimManager.Instance.GetAllCoin();
    }

    public void MoveStart()
    {
        // ī�忡 �ǹ� ȿ�� ����
        for (int i = moveIndex; i < moveIndex + 4; i++)
        {
            MapManager.Instance.fieldList[i].OnAccessCard();
        }

        isMoving = true;
    }

    public void MoveEnd()
    {
        moveCount = 0;
        isMoving = false;

        // ����������� ī�� ����

        // ���� 4���� �ʵ�
        for (int i = moveIndex - 4; i < moveIndex; i++)
        {
            // (fieldType = not)
            MapManager.Instance.fieldList[i].fieldType = FieldType.not;

            // drag and drop ���ϰ�
            MapManager.Instance.fieldList[i].dragbleCard.canDragAndDrop = false;

            MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.originFieldSprite;
        }

        // ���� �ʵ�(fieldType ����)
        for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
        {
            if (MapManager.Instance.fieldList[i].fieldType == FieldType.yet)
            {
                MapManager.Instance.fieldList[i].fieldType = FieldType.able;
            }

            MapManager.Instance.fieldList[i].background.sprite = ConstManager.Instance.nowFieldSprite;
        }
    }

    public void Move()
    {
        // �����̱�
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            Vector3 movePos = MapManager.Instance.fieldList[moveIndex].transform.position;
            player.transform.DOMove(movePos, 0.25f);
            //player.Move(MapManager.Instance.fieldRectList[moveIndex].transform.position, 0.25f);

        });
        sequence.AppendInterval(0.25f);

        // �����ī�� ȿ�� �ߵ�
        sequence.AppendCallback(() =>
        {
            if (MapManager.Instance.fieldList[moveIndex].cardPower.cardType != CardType.NULL)
            {
                MapManager.Instance.fieldList[moveIndex].accessBeforeOnField?.Invoke(player, MapManager.Instance.fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(moveDuration);

        // �÷��̾����� �ʵ� ȿ�� ���뤷
        sequence.AppendCallback(() =>
        {
            if (MapManager.Instance.fieldList[moveIndex].cardPower.cardType != CardType.NULL)
            {
                player.OnFeild(MapManager.Instance.fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(moveDuration);

        // �÷��̾����� �ǹ�ȿ�� ����
        sequence.AppendCallback(() =>
        {
            MapManager.Instance.fieldList[moveIndex].accessBuildToPlayerAfterOnField?.Invoke(player);
            //Debug.Log("player apply build");
        });
        sequence.AppendInterval(moveDuration);

        sequence.AppendCallback(() => {
            moveIndex++;
            moveCount++;

            NextAction();
        });
    }

    public void OnClickMove()
    {
        // move���϶� �� next�� ������ ���ϰ�
        if (isMoving == false)
        {
            for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                // ���� �� ��ġ�������� move �ȵ�
                if(MapManager.Instance.fieldList[i].cardPower == null)
                {
                    return;
                }
            }

            NextAction();
        }
    }

    public void NextAction()
    {
        // TurnEnd
        if (moveIndex == MapManager.Instance.fieldList.Count)
        {
            Debug.Log("Loop End");
            TurnEnd();

            TurnEndEvent.Occurred();

            return;
        }

        // move start
        if (moveCount == 0)
        {
            MoveStartEvent.Occurred();

            MoveStart();
        }

        // move end
        if (moveCount == maxMoveCount)
        {
            MoveEnd();

            // ī�� �̱�
            MoveEndEvent.Occurred();

            return;
        }


        Move();
    }
}    