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

    private BuffController buffCon;

    private void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        buffCon = GetComponent<BuffController>();

        if (rectTrm == null)
        {
            Debug.LogError("player rectTrm is null");
        }
    }

    void Start()
    {
        hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
        swordValue.RuntimeValue = 0;
        shieldValue.RuntimeValue = 0;

        playerValueChangeEvent.Occurred();
    }

    public void OnFeild(Field field)
    {
        buffCon.UseBuffs(UseTiming.AfterMove, 0);

        switch (field.cardPower.cardType)
        {
            case CardType.Potion:
                OnPotion(field);
                break;

            case CardType.Sword:
                OnSword(field);
                break;

            case CardType.Sheild:
                OnShield(field);
                break;

            case CardType.Monster:
                OnMonster(field);
                break;

            
            default:
                Debug.LogError("ī�� Ÿ���� null");
                break;
        }

        playerValueChangeEvent.Occurred();
        

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower.cardType);

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
        if(field.cardPower.value > 0)
        {
            AddFieldBuff(field.cardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetPotion, field.cardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�

            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + field.cardPower.value, 0, hpValue.RuntimeMaxValue);

            buffCon.UseBuffs(UseTiming.AfterGetPotion, field.cardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�
        }
    }

    private void OnSword(Field field)
    {
        if(field.cardPower.value > 0)
        {
            AddFieldBuff(field.cardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetSword, field.cardPower.value);

            swordValue.RuntimeValue = field.cardPower.value;

            buffCon.UseBuffs(UseTiming.AfterGetSword, field.cardPower.value);
        }
    }

    private void OnShield(Field field)
    {
        if(field.cardPower.value > 0)
        {
            AddFieldBuff(field.cardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetShield, field.cardPower.value);

            int addShieldValue = field.cardPower.value + shieldValue.RuntimeValue;

            shieldValue.RuntimeValue = field.cardPower.value;

            buffCon.UseBuffs(UseTiming.AfterGetShield, addShieldValue);
        }
    }

    private void OnMonster(Field field)
    {
        int currentMonsterValue = field.cardPower.value;

        if(currentMonsterValue > 0 && swordValue.RuntimeValue > 0) // ���� ���ݷ��� 0 �̻��̰� Į�� ������
        {
            buffCon.UseBuffs(UseTiming.BeforeSword, field.cardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�

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
    }
}