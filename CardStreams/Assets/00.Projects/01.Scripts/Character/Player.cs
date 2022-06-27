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

    public void CheckPlayerAlive() // 플레이어 쓰러졌는지 검사하는 메소드 | PlayerValueChanged에 넣으면 처음 Init때 걸려서 안됨
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
                Debug.LogError("카드 타입이 null");
                break;
        }

        playerValueChangeEvent.Occurred();
        

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower as BasicCard);

    }

    private void AddFieldBuff(List<Buff> buffList) // 필드의 버프들을 버프컨트롤러에 추가
    {
        foreach (Buff buff in buffList)
        {
            buffCon.AddBuff(buff);
        }
    }

    private void OnPotion(Field field) // 체력증가포션
    {
        BasicCard cardPower = (field.cardPower as BasicCard);

        if (cardPower.value > 0)
        {
            AddFieldBuff(cardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetPotion, cardPower.value); // 0은 아무의미도없음 int? 나중에

            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + cardPower.value, 0, hpValue.RuntimeMaxValue);

            buffCon.UseBuffs(UseTiming.AfterGetPotion, cardPower.value); // 0은 아무의미도없음 int? 나중에
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

        if(currentMonsterValue > 0 && swordValue.RuntimeValue > 0) // 몬스터 공격력이 0 이상이고 칼이 있으면
        {
            buffCon.UseBuffs(UseTiming.BeforeSword, cardPower.value); // 0은 아무의미도없음 int? 나중에

            currentMonsterValue -= swordValue.RuntimeValue;

            buffCon.UseBuffs(UseTiming.AfterSword, currentMonsterValue);
        }


        // 방패 쓰는지 검사

        int leftShieldValue = shieldValue.RuntimeValue;

        if (currentMonsterValue > 0 && shieldValue.RuntimeValue > 0) // 남은 몬스터 공격력이 0 이상이고 방패가 있으면
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
        currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, 99);
        hpValue.RuntimeValue -= currentMonsterValue;
        playerValueChangeEvent.Occurred();

        killMobCount++;
    }
}