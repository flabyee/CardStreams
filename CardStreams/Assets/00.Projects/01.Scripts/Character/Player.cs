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
                Debug.Log("ī�� Ÿ���� null");
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

    private void OnPotion(Field field)
    {
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + field.cardPower.value, 0, hpValue.RuntimeMaxValue);
        AddFieldBuff(field.cardPower.buffList);
    }

    private void OnSword(Field field)
    {
        swordValue.RuntimeValue = Mathf.Clamp(field.cardPower.value, 0, swordValue.RuntimeMaxValue);
        AddFieldBuff(field.cardPower.buffList);
    }

    private void OnShield(Field field)
    {
        shieldValue.RuntimeValue = Mathf.Clamp(field.cardPower.value, 0, shieldValue.RuntimeMaxValue);
        AddFieldBuff(field.cardPower.buffList);
    }

    private void OnMonster(Field field)
    {
        GameManager.Instance.AddScore(field.cardPower.value * 2);


        buffCon.UseBuffs(UseTiming.BeforeSword, -1); // 0�� �ƹ��ǹ̵����� int? ���߿�

        int currentMonsterValue = field.cardPower.value - swordValue.RuntimeValue;

        buffCon.UseBuffs(UseTiming.AfterSword, currentMonsterValue);

        // ���� ������ �˻�

        int leftShieldValue = shieldValue.RuntimeValue;

        if (currentMonsterValue > 0) // 0 ���ϸ� �����ʿ����
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
        currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, hpValue.RuntimeMaxValue);
        hpValue.RuntimeValue -= currentMonsterValue;
        playerValueChangeEvent.Occurred();
    }
}