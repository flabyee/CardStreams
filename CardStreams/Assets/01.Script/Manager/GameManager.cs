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
    private int moveCount = 0;  // n번씩 움직일거다
    
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

            // 모든 필드의 필드타입 yet으로
            foreach (Field field in fieldList)
            {
                field.fieldType = FieldType.yet;
            }

            // 턴시작시 몬스터? 설치불가타일? 생성
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

                    // 이걸 해주는 이유는 drop할때 able || randomMob 인지 체크하기 때문이다
                    fieldList[i].fieldType = FieldType.randomMob;  

                    fieldList[i].dropArea.TriggerOnDrop(dragbleCard);

                    //EffectManager.Instance.GetSpawnMobEffect(fieldList[i].transform.position);
                }
            }

            // 앞에 3칸 활성화
            for (int i = 0; i < maxMoveCount; i++) 
            {
                if(fieldList[i].fieldType == FieldType.yet)
                {
                    fieldList[i].fieldType = FieldType.able;
                }
            }

            // change mode 활성화
            //isChange = true;

            // 카드 뽑기
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

            // NextTurnEvent에서 TurnStart를 해준다

            TurnEndEvent.Occurred();

            return;
        }


        // move start
        if (moveCount == 0)
        {
            MoveStartEvent.Occurred();

            // To Do : 카드에 건물 효과 적용
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

            // 사용하지않은 카드 제거

            // 이전 필드(fieldType = not)
            for (int i = 0; i < moveIndex; i++)
            {
                fieldList[i].fieldType = FieldType.not;
            }
            // 다음 필드(fieldType 변경)
            for(int i = moveIndex; i < moveIndex + maxMoveCount; i++)
            {
                if(fieldList[i].fieldType == FieldType.yet)
                {
                    fieldList[i].fieldType = FieldType.able;
                }
            }

            // To Do : 카드 뽑기
            MoveEndEvent.Occurred();

            return;
        }


        // 움직이기
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => {
            Vector3 movePos = fieldList[moveIndex].transform.position;
            movePos.z = 0;
            player.transform.DOMove(movePos, 0.25f);
            //player.Move(fieldList[moveIndex].transform.position, 0.25f);

        });
        sequence.AppendInterval(0.25f);

        // 스페셜카드 효과 발동
        sequence.AppendCallback(() => {
            if (fieldList[moveIndex].cardType != CardType.NULL)
            {
                fieldList[moveIndex].accessBeforeOnField?.Invoke(player, fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(duration);

        // 플레이어한테 필드 효과 적용ㅇ
        sequence.AppendCallback(() => {
            if (fieldList[moveIndex].cardType != CardType.NULL)
            {
                player.OnFeild(fieldList[moveIndex]);
            }
        });
        sequence.AppendInterval(duration);

        // 플레이어한테 건물효과 적용
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