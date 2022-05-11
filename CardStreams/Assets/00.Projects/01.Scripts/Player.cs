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

    public Func<int, int> OnSword;  // 남은 몬스터의 피 | 계산 후 데미지(return)
    public Func<bool> IsDeleteSwordBuff; // 공격 후 버프를 남겨둘까요?

    public Func<int, int> OnShield; // 남은 몬스터의 피 | 계산 후 데미지(return)
    public Func<bool> IsDeleteShieldBuff; // 방어 후 버프를 남겨둘까요?

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
                    Debug.Log("없음");
                    swordAttackDamage = field.cardPower.value - swordValue.RuntimeValue; // 평범한 칼
                    swordValue.RuntimeValue = 0;
                }
                else
                {
                    swordAttackDamage = (OnSword?.Invoke(field.cardPower.value)).Value; // 데미지 = 칼 함수실행(플레이어, 몬스터 피)
                }

                if(IsDeleteSwordBuff != null) // 버프 한번쓰고 지울거면
                {
                    OnSword = null; // 검에 발린 버프를 싹 지움
                }

                //damage -= swordValue.RuntimeValue;
                //swordValue.RuntimeValue = 0;

                if (swordAttackDamage > 0) // 칼이 못막아서 데미지가 0보다크면 방패계산
                {
                    shieldDefenseDamage = swordAttackDamage - shieldValue.RuntimeValue; // 방패계산데미지 = 검으로까인데미지 - 방패수치

                    if(OnShield == null)
                    {
                        if (shieldDefenseDamage < 0) // 방패로막은 데미지가 0보다 작으면 방패가 버팀
                        {
                            shieldValue.RuntimeValue -= swordAttackDamage; // 방패 까고
                            shieldDefenseDamage = 0; // 데미지 0
                        }
                        else // 방패로막은 데미지가 0보다 크면 방패가 사라짐
                        {
                            shieldDefenseDamage -= shieldValue.RuntimeValue; // 데미지 까고
                            shieldValue.RuntimeValue = 0; // 방패 0
                        }
                    }
                    else
                    {
                        shieldDefenseDamage = (OnShield?.Invoke(swordAttackDamage)).Value; // 최종데미지 = 방패 함수실행(플레이어, 칼맞은 몬스터피)
                    }

                    if (IsDeleteShieldBuff != null) // 버프 한번쓰고 지울거면
                    {
                        OnShield = null; // 방패에 발린 버프를 싹 지움
                    }

                    //if (shieldDefenseDamage <= 0) // 최종 데미지가 0보다 작으면
                    //{
                    //    shieldValue.RuntimeValue -= shieldDefenseDamage;
                    //    damage = 0;
                    //}
                    //else
                    //{
                    //    // damage -= shieldValue.RuntimeValue;
                    //    damage = shieldDefenseDamage; // 최종데미지 = 방패로막고남은데미지
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