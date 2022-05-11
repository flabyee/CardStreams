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

    public Func<int, int> OnSword;  // ���� ������ �� | ��� �� ������(return)
    public Func<bool> IsDeleteSwordBuff; // ���� �� ������ ���ܵѱ��?

    public Func<int, int> OnShield; // ���� ������ �� | ��� �� ������(return)
    public Func<bool> IsDeleteShieldBuff; // ��� �� ������ ���ܵѱ��?

    private void Awake()
    {
        rectTrm = GetComponent<RectTransform>();

        if(rectTrm == null)
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
                hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + field.cardPower.value, 0, hpValue.RuntimeMaxValue);
                break;
            case CardType.Sword:
                if(field.cardPower.value > 0)
                {
                    swordValue.RuntimeValue = Mathf.Clamp(field.cardPower.value, 0, swordValue.RuntimeMaxValue);
                    foreach (BuffSO buff in field.cardPower.buffList)
                    {
                        OnSword += buff.BuffOn;
                        IsDeleteSwordBuff += buff.BuffOff;
                    }
                }

                break;
            case CardType.Sheild:
                if (field.cardPower.value > 0)
                {
                    shieldValue.RuntimeValue = Mathf.Clamp(field.cardPower.value, 0, shieldValue.RuntimeMaxValue);
                    foreach (BuffSO buff in field.cardPower.buffList)
                    {
                        OnShield += buff.BuffOn;
                        IsDeleteShieldBuff += buff.BuffOff;
                    }
                }
                break;
            case CardType.Monster:
                GameManager.Instance.AddScore(field.cardPower.value * 2);

                int swordAttackDamage = 0;
                int shieldDefenseDamage = 0;

                if(OnSword == null)
                {
                    Debug.Log("����");
                    swordAttackDamage = field.cardPower.value - swordValue.RuntimeValue; // ����� Į
                    swordValue.RuntimeValue = 0;
                }
                else
                {
                    swordAttackDamage = (OnSword?.Invoke(field.cardPower.value)).Value; // ������ = Į �Լ�����(�÷��̾�, ���� ��)
                }

                if(IsDeleteSwordBuff != null) // ���� �ѹ����� ����Ÿ�
                {
                    OnSword = null; // �˿� �߸� ������ �� ����
                }

                //damage -= swordValue.RuntimeValue;
                //swordValue.RuntimeValue = 0;

                if (swordAttackDamage > 0) // Į�� �����Ƽ� �������� 0����ũ�� ���а��
                {
                    shieldDefenseDamage = swordAttackDamage - shieldValue.RuntimeValue; // ���а�굥���� = �����α��ε����� - ���м�ġ

                    if(OnShield == null)
                    {
                        if (shieldDefenseDamage < 0) // ���зθ��� �������� 0���� ������ ���а� ����
                        {
                            shieldValue.RuntimeValue -= swordAttackDamage; // ���� ���
                            shieldDefenseDamage = 0; // ������ 0
                        }
                        else // ���зθ��� �������� 0���� ũ�� ���а� �����
                        {
                            shieldDefenseDamage -= shieldValue.RuntimeValue; // ������ ���
                            shieldValue.RuntimeValue = 0; // ���� 0
                        }
                    }
                    else
                    {
                        shieldDefenseDamage = (OnShield?.Invoke(swordAttackDamage)).Value; // ���������� = ���� �Լ�����(�÷��̾�, Į���� ������)
                    }

                    if (IsDeleteShieldBuff != null) // ���� �ѹ����� ����Ÿ�
                    {
                        OnShield = null; // ���п� �߸� ������ �� ����
                    }

                    //if (shieldDefenseDamage <= 0) // ���� �������� 0���� ������
                    //{
                    //    shieldValue.RuntimeValue -= shieldDefenseDamage;
                    //    damage = 0;
                    //}
                    //else
                    //{
                    //    // damage -= shieldValue.RuntimeValue;
                    //    damage = shieldDefenseDamage; // ���������� = ���зθ�����������
                    //    shieldValue.RuntimeValue = 0;
                    //}
                }

                shieldDefenseDamage = Mathf.Clamp(shieldDefenseDamage, 0, hpValue.RuntimeMaxValue);
                hpValue.RuntimeValue -= shieldDefenseDamage;

                break;
            case CardType.Coin:
                GameManager.Instance.AddScore(2);
                break;
            case CardType.Special:

            default:
                break;
        }

        playerValueChangeEvent.Occurred();

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower.cardType);

    }
}