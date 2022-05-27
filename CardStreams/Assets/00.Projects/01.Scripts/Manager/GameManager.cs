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

    private bool isMoving;  // move���϶� �� next�� ������ ���ϰ�

    [HideInInspector]public int rerollCount;

    public bool canStartTurn;

    [Header("UI")]
    public Player player;

    [Header("System")]
    [SerializeField] float moveDuration;
    [SerializeField] float fieldResetDelay;
    private int maxMoveCount = 3;  // n
    private int moveIndex = 0;
    private int moveCount = 0;  // n���� �����ϰŴ�

    [Header("StageData")]
    private int mobSpawnAmount;
    private int mobSpawnIncreaseAmount;
    private int mobAttackAmount;
    private int mobAttackIncreaseAmount;

    [Header("Controller")]
    private FieldController fieldController = new FieldController();


    [Header("IntValue")]
    public IntValue goldValue;
    public IntValue turnCountValue;
    

    [Header("Event")]
    public EventSO TurnStartEvent;
    public EventSO TurnEndEvent;
    public EventSO MoveStartEvent;
    public EventSO MoveEndEvent;

    public EventSO goldChangeEvent;
    public EventSO playerDieEvent;


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
        LoadStageData();

        goldValue.RuntimeValue += 20;
        goldChangeEvent.Occurred();

        //TurnStart();
    }

    public void LoadStageData()
    {
        StageDataSO stageData = DataManager.Instance.GetNowStageData();
        maxMoveCount = stageData.moveCount;
        
    }

    public void AddGold(int amount)
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

    // To Do : ���߿� ����
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
            fieldController.SetAllFieldYet();

            // ���� �� ����


            // �տ� nĭ Ȱ��ȭ
            fieldController.SetNextFieldAble(moveIndex);

            // change mode Ȱ��ȭ
            //isChange = true;

            // ��ġ ����

            TurnStartEvent.Occurred();
        }
    }

    public void TurnEnd()
    {
        // ���� 4���� �ʵ�
        fieldController.SetBeforeFieldNot(moveIndex);

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
        fieldController.BuildAccessNextField(moveIndex);

        isMoving = true;
    }

    public void MoveEnd()
    {
        moveCount = 0;
        isMoving = false;

        // ����������� ī�� ����

        // ���� 4���� �ʵ�
        fieldController.SetBeforeFieldNot(moveIndex);

        // ���� �ʵ�(fieldType ����)
        fieldController.SetNextFieldAble(moveIndex);
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

            // �÷��̾� �׾����� ��
            if (player.isAlive == false)
            {
                Debug.Log("�÷��̾� ����");
                playerDieEvent.Occurred();
                return;
            }

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


        DropArea dropArea = MapManager.Instance.fieldList[tempIndex].dropArea;

        // drop area ����
        dragbleCard.SetDroppedArea(dropArea);

        // �θ� ����
        dragbleCard.transform.SetParent(dropArea.rectTrm, true);

        // ���� ����
        CardPower cardPower = dragbleCard.GetComponent<CardPower>();

        dropArea.field.cardPower = cardPower;
        dropArea.field.dragbleCard = dragbleCard;
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