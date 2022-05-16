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
                Debug.Log("카드 타입이 null");
                break;
        }

        playerValueChangeEvent.Occurred();

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower.cardType);

    }

    private void AddFieldBuff(List<Buff> buffList) // 필드의 버프들을 버프컨트롤러에 추가
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


        buffCon.UseBuffs(UseTiming.BeforeSword, -1); // 0은 아무의미도없음 int? 나중에

        int currentMonsterValue = field.cardPower.value - swordValue.RuntimeValue;

        buffCon.UseBuffs(UseTiming.AfterSword, currentMonsterValue);

        // 방패 쓰는지 검사

        int leftShieldValue = shieldValue.RuntimeValue;

        if (currentMonsterValue > 0) // 0 이하면 방패필요없음
        {
            buffCon.UseBuffs(UseTiming.BeforeShield, currentMonsterValue);

            if (currentMonsterValue - shieldValue.RuntimeValue > 0) // 몬스터가더크면
            {
                currentMonsterValue -= shieldValue.RuntimeValue; // 몬스터 -= 방패
                leftShieldValue = 0; // 방패 0
            }
            else // 방패가더크면
            {
                leftShieldValue -= currentMonsterValue; // 방패 -= 몬스터
                currentMonsterValue = 0; // 피해 0
            }

            buffCon.UseBuffs(UseTiming.AfterShield, currentMonsterValue);
        }


        // 여기부터는 데미지 적용

        // 칼0 (쌍검 예외)
        swordValue.RuntimeValue = 0;

        // 방패 남은거
        shieldValue.RuntimeValue = leftShieldValue;

        // 실제데미지 적용
        currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, hpValue.RuntimeMaxValue);
        hpValue.RuntimeValue -= currentMonsterValue;
        playerValueChangeEvent.Occurred();
    }
}