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

    [Header("UI")]
    public IntValue goldValue;
    public EventSO goldChangeEvent;

    public GameObject deckSeePanel;
    public RectTransform deckSeePanelTrm;
    public GameObject seeCardPrefab;
    public GameObject cardPrefab;
    private bool isDeckSee;
    private int monsterInt;
    private int hpInt;
    public Text hpIntText;
    public Text mobIntText;

    public GameObject turnEndPanel;
    


    [Header("System")]
    public Player player;
    private bool isMoving;
    [SerializeField] float duration;
    public List<Field> fieldList;
    private int moveIndex = 0;
    private int moveCount = 0;  // n���� �����ϰŴ�
    
    [SerializeField] int maxMoveCount = 3;  // n


    public IntValue turnCountValue;

    [HideInInspector]public int rerollCount;


    // Actions

    public EventSO GameStartEvent;
    public EventSO TurnStartEvent;
    public EventSO TurnEndEvent;
    public EventSO MoveStartEvent;
    public EventSO MoveEndEvent;


    private void Awake()
    {
        Instance = this;

        isDeckSee = false;
    }

    private void Start()
    {
        GameStartEvent.Occurred();

        goldValue.RuntimeValue += 20;
        goldChangeEvent.Occurred();

        //TurnStart();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            MoveToNextFeild();
        }
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

    public void OnClickReset()
    {
        DropArea.dropAreas.Clear();
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClickDeckSee()
    {
        isDeckSee = !isDeckSee;
        deckSeePanel.gameObject.SetActive(isDeckSee);
        player.gameObject.SetActive(!isDeckSee);

        if (isDeckSee == true)
        {
            foreach (RectTransform item in deckSeePanelTrm)
            {
                Destroy(item.gameObject);
            }

            monsterInt = 0;
            hpInt = 0;

            foreach (var item in HandleManager.Instance.deck)
            {
                Instantiate(seeCardPrefab, deckSeePanelTrm);
                CardPower cardPower = seeCardPrefab.GetComponent<CardPower>();
                cardPower.cardType = item.cardType;
                cardPower.value = item.value;
                cardPower.ApplyUI();

                

                if (item.cardType == CardType.Monster)
                {
                    monsterInt += item.value;
                }

                if (item.cardType == CardType.Potion 
                    || item.cardType == CardType.Sheild 
                    || item.cardType == CardType.Sword)
                {
                    hpInt += item.value;
                }
            }

            hpIntText.text = $"hp : {hpInt}";
            mobIntText.text = $"mob : {monsterInt}";
        }
    }

    public void TurnStart()
    {
        if (moveIndex == 0)
        {
            turnCountValue.RuntimeValue += turnCountValue.RuntimeValue + 1;

            // ��� �ʵ��� �ʵ�Ÿ�� yet����
            foreach (Field field in fieldList)
            {
                field.fieldType = FieldType.yet;
            }

            // �Ͻ��۽� ����? ��ġ�Ұ�Ÿ��? ����
            bool[] isMonster = new bool[30];
            for (int i = 1; i < fieldList.Count - 1; i++)
            {
                if (i < turnCountValue.RuntimeValue + 1)
                    isMonster[i] = true;
                else
                    isMonster[i] = false;
            }
            for (int i = 1; i < 18; i++)
            {
                int j = UnityEngine.Random.Range(1, fieldList.Count - 1);
                bool temp = isMonster[i];
                isMonster[i] = isMonster[j];
                isMonster[j] = temp;
            }
            for (int i = 1; i < fieldList.Count - 1; i++)
            {
                if (isMonster[i] == true)
                {
                    //fieldList[i].fieldType = FieldType.not;
                    //fieldList[i].cardType = CardType.Monster;

                    int value = 5;  //UnityEngine.Random.Range(1, 6)
                    //fieldList[i].value = value;

                    //GameObject seeObj = Instantiate(seeCardPrefab, fieldList[i].transform);
                    //seeObj.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
                    //seeObj.transform.Find("face").GetComponent<Image>().sprite = ConstManager.Instance.monsterSprite;
                    //fieldList[i].cardPower = seeObj.GetComponent<CardPower>();

                    GameObject cardObj = Instantiate(cardPrefab, fieldList[i].transform);
                    DragbleCard dragbleCard = cardObj.GetComponent<DragbleCard>();

                    dragbleCard.SetData_Feild(CardType.Monster, value);
                    dragbleCard.canDragAndDrop = false;

                    // �̰� ���ִ� ������ drop�Ҷ� able || randomMob ���� üũ�ϱ� �����̴�
                    fieldList[i].fieldType = FieldType.randomMob;  

                    fieldList[i].dropArea.TriggerOnDrop(dragbleCard);

                    //EffectManager.Instance.GetSpawnMobEffect(fieldList[i].transform.position);
                }
            }

            // �տ� 3ĭ Ȱ��ȭ
            for (int i = 0; i < maxMoveCount; i++) 
            {
                if(fieldList[i].fieldType == FieldType.yet)
                {
                    fieldList[i].fieldType = FieldType.able;
                }
            }

            // change mode Ȱ��ȭ
            //isChange = true;

            // ī�� �̱�
            TurnStartEvent.Occurred();

        }
    }

    public void OnClickMove()
    {
        if(isMoving == false)
        {
            for (int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                if(fieldList[i].fieldType != FieldType.not)
                {
                    return;
                }
            }

            MoveToNextFeild();
        }
    }

    public void MoveToNextFeild()
    {
        // TurnEnd
        if (moveIndex == fieldList.Count)
        {
            moveIndex = 0;
            moveCount = 0;
            isMoving = false;

            // NextTurnEvent���� TurnStart�� ���ش�

            TurnEndEvent.Occurred();

            return;
        }


        // move start
        if (moveCount == 0)
        {
            MoveStartEvent.Occurred();

            // To Do : ī�忡 �ǹ� ȿ�� ����
            for(int i = moveIndex; i < moveIndex + 4; i++)
            {
                fieldList[i].OnAccessCard();
            }

            isMoving = true;
        }


        // move end
        if (moveCount == maxMoveCount)
        {
            moveCount = 0;
            isMoving = false;

            // ����������� ī�� ����

            // ���� �ʵ�(fieldType = not)
            for (int i = 0; i < moveIndex; i++)
            {
                fieldList[i].fieldType = FieldType.not;
            }
            // ���� �ʵ�(fieldType ����)
            for(int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                if(fieldList[i].fieldType == FieldType.yet)
                {
                    fieldList[i].fieldType = FieldType.able;
                }
            }

            // To Do : ī�� �̱�
            MoveEndEvent.Occurred();

            return;
        }


        // �����̱�
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => {
            Vector3 movePos = fieldList[moveIndex].transform.position;
            movePos.z = 0;
            player.transform.DOMove(movePos, 0.25f);
            //player.Move(fieldList[moveIndex].transform.position, 0.25f);

        });
        sequence.AppendInterval(0.25f);

        // �����ī�� ȿ�� �ߵ�
        sequence.AppendCallback(() => {
            if (fieldList[moveIndex].cardType != CardType.NULL)
            {
                fieldList[moveIndex].accessBeforeOnField?.Invoke(player, fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(duration);

        // �÷��̾����� �ʵ� ȿ�� ���뤷
        sequence.AppendCallback(() => {
            if (fieldList[moveIndex].cardType != CardType.NULL)
            {
                player.OnFeild(fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(duration);

        // �÷��̾����� �ǹ�ȿ�� ����
        sequence.AppendCallback(() => {
            fieldList[moveIndex].accessBuildToPlayerAfterOnField?.Invoke(player);
            Debug.Log("player apply build");
        });
        sequence.AppendInterval(duration);

        sequence.AppendCallback(() => {
            moveIndex++;
            moveCount++;
            MoveToNextFeild();
        });
    }
}