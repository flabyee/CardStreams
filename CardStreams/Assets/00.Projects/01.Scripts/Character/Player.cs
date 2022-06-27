using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    private RectTransform rectTrm;

    public EventSO playerValueChangeEvent;
    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;
    public IntValue goldValue;
   

    public bool isAlive { get; private set; }

    private BuffController buffCon;

    [Header("Debug")]
    public bool isDontDie;

    public int killMobCount;

    private void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        buffCon = GetComponent<BuffController>();

        if (rectTrm == null)
        {
            Debug.LogError("player rectTrm is null");
        }

        // IntValue Init
        hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
        swordValue.RuntimeValue = 0;
        shieldValue.RuntimeValue = 0;
        goldValue.RuntimeValue = 0;
    }


    void Start()
    {

        playerValueChangeEvent.Occurred();

        isAlive = true;
    }

    public void CheckPlayerAlive() // �÷��̾� ���������� �˻��ϴ� �޼ҵ� | PlayerValueChanged�� ������ ó�� Init�� �ɷ��� �ȵ�
    {
        if(isDontDie == true)
        {
            return;
        }

        isAlive = (hpValue.RuntimeValue > 0) ? true : false;
    }

    public void OnFeild(Field field)
    {
        buffCon.UseBuffs(UseTiming.AfterMove, 0);

        if(field.cardPower.cardType == CardType.Basic)
        {
            
        }

        switch ((field.cardPower as BasicCard).basicType)
        {
            case BasicType.Potion:
                OnPotion(field);
                break;

            case BasicType.Sword:
                OnSword(field);
                break;

            case BasicType.Sheild:
                OnShield(field);
                break;

            case BasicType.Monster:
                OnMonster(field);
                break;

            
            default:
                Debug.LogError("ī�� Ÿ���� null");
                break;
        }

        playerValueChangeEvent.Occurred();
        

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower as BasicCard);

    }

    private void AddFieldBuff(List<Buff> buffList) // �ʵ��� �������� ������Ʈ�ѷ��� �߰�
    {
        foreach (Buff buff in buffList)
        {
            buffCon.AddBuff(buff);
        }
    }

    private void OnPotion(Field field) // ü����������
    {
        BasicCard cardPower = (field.cardPower as BasicCard);

        if (cardPower.value > 0)
        {
            AddFieldBuff(cardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetPotion, cardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�

            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + cardPower.value, 0, hpValue.RuntimeMaxValue);

            buffCon.UseBuffs(UseTiming.AfterGetPotion, cardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�
        }
    }

    private void OnSword(Field field)
    {
        BasicCard cardPower = (field.cardPower as BasicCard);

        if (cardPower.value > 0)
        {
            AddFieldBuff(cardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetSword, cardPower.value);

            swordValue.RuntimeValue = cardPower.value;

            buffCon.UseBuffs(UseTiming.AfterGetSword, cardPower.value);
        }
    }

    private void OnShield(Field field)
    {
        BasicCard cardPower = (field.cardPower as BasicCard);

        if (cardPower.value > 0)
        {
            AddFieldBuff(cardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetShield, cardPower.value);

            int addShieldValue = cardPower.value + shieldValue.RuntimeValue;

            shieldValue.RuntimeValue = cardPower.value;

            buffCon.UseBuffs(UseTiming.AfterGetShield, addShieldValue);
        }
    }

    private void OnMonster(Field field)
    {
        BasicCard cardPower = (field.cardPower as BasicCard);

        int currentMonsterValue = cardPower.value;

        if(currentMonsterValue > 0 && swordValue.RuntimeValue > 0) // ���� ���ݷ��� 0 �̻��̰� Į�� ������
        {
            buffCon.UseBuffs(UseTiming.BeforeSword, cardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�

            currentMonsterValue -= swordValue.RuntimeValue;

            buffCon.UseBuffs(UseTiming.AfterSword, currentMonsterValue);
        }


        // ���� ������ �˻�

        int leftShieldValue = shieldValue.RuntimeValue;

        if (currentMonsterValue > 0 && shieldValue.RuntimeValue > 0) // ���� ���� ���ݷ��� 0 �̻��̰� ���а� ������
        {
            buffCon.UseBuffs(UseTiming.BeforeShield, currentMonsterValue);

            if (currentMonsterValue - shieldValue.RuntimeValue > 0) // ���Ͱ���ũ��
            {
                currentMonsterValue -= shieldValue.RuntimeValue; // ���� -= ����
                leftShieldValue = 0; // ���� 0
            }
            else // ���а���ũ��
            {
                leftShieldValue -= currentMonsterValue; // ���� -= ����
                currentMonsterValue = 0; // ���� 0
            }

            buffCon.UseBuffs(UseTiming.AfterShield, currentMonsterValue);
        }


        // ������ʹ� ������ ����

        // Į0 (�ְ� ����)
        swordValue.RuntimeValue = 0;

        // ���� ������
        shieldValue.RuntimeValue = leftShieldValue;

        // ���������� ����
        currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, 99);
        hpValue.RuntimeValue -= currentMonsterValue;
        playerValueChangeEvent.Occurred();

        killMobCount++;
    }
}