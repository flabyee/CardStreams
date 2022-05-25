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

    public bool isDebuging;

    [Header("System")]
    [SerializeField] float moveDuration;
    [SerializeField] float fieldResetDelay;
    private bool isMoving;  // move���϶� �� next�� ������ ���ϰ�

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

        if(isDebuging == true)
        {
            moveDuration = 0.05f;
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
        List<int> canSpawnList = new List<int>();
        List<int> deleteFieldList = new List<int>();

        for (int i = 0; i < MapManager.Instance.fieldList.Count; i++)
        {
            if (i == 0) // 0��°ĭ ��� ����
            {
                deleteFieldList.Add(0);
                continue;
            }

            canSpawnList.Add(i);
        }

        for (int i = 0; i < mobSpawnAmount; i++)
        {
            // ������ ������ �Ȼ��� �ֵ��� �־��ֱ�
            if(canSpawnList.Count <= 0)
            {

                // �����ϰ� ����
                for (int j = 0; j < deleteFieldList.Count; j++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, deleteFieldList.Count);
                    int temp = deleteFieldList[j];
                    deleteFieldList[j] = deleteFieldList[randomIndex];
                    deleteFieldList[randomIndex] = temp;
                }

                // ���� ����
                for(int j = i; j < mobSpawnAmount; j++)
                {
                    // mobSpawnAmount >= Ÿ�ϰ����� ���̻������, ex) 20���϶� 20>=20�̸� ����Ʈ[20] null

                    if (j >= MapManager.Instance.fieldList.Count)
                    {
                        return;
                    }
                    CreateEnemy(deleteFieldList[j - i]);
                }

                break;
            }

            // 10
            int randIndex = canSpawnList[UnityEngine.Random.Range(0, canSpawnList.Count)];

            // ���õȸ���Ʈ�� �߰�
            CreateEnemy(randIndex);

            // ������ + �����ű�ó ����
            canSpawnList.Remove(randIndex);
            if (canSpawnList.Contains(randIndex + 1))
            {

                deleteFieldList.Add(randIndex + 1);
                canSpawnList.Remove(randIndex + 1);
            }
            if (canSpawnList.Contains(randIndex - 1))
            {

                deleteFieldList.Add(randIndex - 1);
                canSpawnList.Remove(randIndex - 1);
            }

        }
    }

    /// <summary> ���� Ư�� ĭ�� ���͸� �����մϴ�. </summary>
    /// <param name="fieldIndex">������ ĭ</param>
    public void CreateEnemy(int fieldIndex)
    {
        int value = mobAttackAmount; // �����Ǵ� ������ ��

        // ���ο� ī�� ����
        GameObject cardObj = Instantiate(cardPrefab, MapManager.Instance.fieldList[fieldIndex].transform);
        DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();
        CardPower cardPower = cardObj.GetComponent<CardPower>();

        // cardPower�� ���� �ֱ�
        dragbleCard.SetData_Feild(CardType.Monster, value);

        // �� �����̰�
        dragbleCard.canDragAndDrop = false;

        // �ʵ忡 ���� + not����
        MapManager.Instance.fieldList[fieldIndex].Init(cardPower, dragbleCard, FieldType.not);

        // ���� ����
        cardPower.backImage.color = Color.magenta;

        // craete effect
        EffectManager.Instance.GetSpawnMobEffect(MapManager.Instance.fieldList[fieldIndex].transform.position);
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
                // GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position);
                GoldAnimManager.Instance.CreateCoin(cardPower.originValue * cardPower.goldP, nowField.transform.position);
                yield return new WaitForSeconds(fieldResetDelay);
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

    public void DropByRightClick(DragbleCard dragbleCard)
    {
        int tempIndex = -1; // 4ĭ�� ����ִ� �ʵ��� �ε����� ���� ��
        for (int i = 0; i < 4; i++)
        {
            if (MapManager.Instance.fieldList[moveIndex + i].dragbleCard == null)
            {
                tempIndex = moveIndex + i;
                break;
            }
        }

        // ����ִ� �� ������ ����
        if (tempIndex == -1)
        {
            return;
        }

        // �θ� ����
        DropArea tempDropArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        dragbleCard.transform.SetParent(tempDropArea.rectTrm, true);

        // ���� ����
        CardPower cardPower = dragbleCard.GetComponent<CardPower>();

        tempDropArea.field.cardPower = cardPower;
        tempDropArea.field.dragbleCard = dragbleCard;
    }

    public void OnClickMove()
    {
        // move���϶� �� next�� ������ ���ϰ�
        if (isMoving == false)
        {
            for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                // ���� �� ��ġ�������� move �ȵ�
                if (MapManager.Instance.fieldList[i].cardPower == null)
                {
                    return;
                }
            }

            NextAction();
        }
    }

    public void OnClickSpeedAdd(float amount)
    {
        moveDuration = Mathf.Clamp(moveDuration + amount, 0.01f, 1f);
    }
}    