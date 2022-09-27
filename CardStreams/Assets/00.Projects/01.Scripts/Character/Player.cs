using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private RectTransform rectTrm;
    private BuffController buffCon;

    

    [Header("IntValue")]
    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;
    public IntValue goldValue;

    [Header("Event")]
    public EventSO playerValueChangeEvent;

    public bool isAlive { get; private set; }

    // 레벨 관련
    private int level = 1;
    private int exp;
    private int nextExp = 30;
    public Action<int, int, int> GetExpEvent;

    // 미션 시스템을 위한


    [Header("Debug")]
    public DebugBoolSO isDontDie;

    

    protected virtual void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        buffCon = GetComponent<BuffController>();

        // IntValue Init : VillagePlayer로 옮겨서 마을에서 수치바꿀수있게
        //hpValue.RuntimeValue = hpValue.InitialMaxValue;
        //hpValue.RuntimeMaxValue = hpValue.InitialMaxValue;

        // 마을에서 시작 안했으면 13/13으로
        if (hpValue.RuntimeMaxValue <= 0)
        {
            hpValue.RuntimeMaxValue = hpValue.InitialMaxValue;
            hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
        }

        swordValue.RuntimeValue = 0;
        shieldValue.RuntimeValue = 0;
    }


    void Start()
    {
        isAlive = true;
    }

    public void CheckPlayerAlive() // 플레이어 쓰러졌는지 검사하는 메소드 | PlayerValueChanged에 넣으면 처음 Init때 걸려서 안됨
    {
        if(isDontDie.b == true)
        {
            return;
        }

        isAlive = (hpValue.RuntimeValue > 0) ? true : false;
    }

    public void OnFeild(Field field)
    {
        buffCon.UseBuffs(UseTiming.AfterMove, 0);

        switch (field.cardPower.basicType)
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

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower);

        // 미션
        MissionObserverManager.instance.OnBasicCard?.Invoke(field.cardPower);
    }

    public bool OnBoss(int damage, out int sword)
    {
        int currentMonsterValue = damage;

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

        // 칼 사용해서 보스에게 추가 피해
        sword = swordValue.RuntimeValue;
        swordValue.RuntimeValue = 0;

        // 방패 남은거
        shieldValue.RuntimeValue = leftShieldValue;

        // 실제데미지 적용
        currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, 99);
        hpValue.RuntimeValue -= currentMonsterValue;
        playerValueChangeEvent.Occurred();


        return hpValue.RuntimeValue <= 0;
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

        // 몬스터의 밸류가 0 이상일 때만
        if (currentMonsterValue > 0)
        {
            // 칼 사용 검사
            if (currentMonsterValue > 0 && swordValue.RuntimeValue > 0) // 몬스터 공격력이 0 이상이고 칼이 있으면
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
                    MissionObserverManager.instance.OnShield?.Invoke(shieldValue.RuntimeValue);

                    currentMonsterValue -= shieldValue.RuntimeValue; // 몬스터 -= 방패
                    leftShieldValue = 0; // 방패 0
                }
                else // 방패가더크면
                {
                    MissionObserverManager.instance.OnShield?.Invoke(currentMonsterValue);

                    leftShieldValue -= currentMonsterValue; // 방패 -= 몬스터
                    currentMonsterValue = 0; // 피해 0
                }

                buffCon.UseBuffs(UseTiming.AfterShield, currentMonsterValue);
            }


            // 칼0 (쌍검 예외)
            swordValue.RuntimeValue = 0;

            // 방패 남은거
            shieldValue.RuntimeValue = leftShieldValue;

            // 실제데미지 적용
            currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, 99);
            hpValue.RuntimeValue -= currentMonsterValue;
            playerValueChangeEvent.Occurred();
        }

        // 킬카운트, 명성 업 (크리스탈은 루프 완료시마다 5개, 게임매니저로 이사감)
        ResourceManager.Instance.AddResource(ResourceType.prestige, 1);

        GetExpBezier(cardPower.originValue);
    }

    public void GetExpBezier(int exp)
    {
        for (int i = 0; i < exp; i++)
        {
            EffectManager.Instance.GetBezierCardEffect(transform.position, ConstManager.Instance.expSprites[UnityEngine.Random.Range(0, ConstManager.Instance.expSprites.Length)],
                TargetType.Exp, () => { GetExp(1); }, 1f, 2f, 2f, false, 2f);
        }
    }
    public void GetExpBezier(int exp, Vector3 pos)
    {
        for (int i = 0; i < exp; i++)
        {
            EffectManager.Instance.GetBezierCardEffect(pos, ConstManager.Instance.expSprites[UnityEngine.Random.Range(0, ConstManager.Instance.expSprites.Length)],
                TargetType.Exp, () => { GetExp(1); }, 1f, 2f, 2f, false, 2f);
        }
    }

    public void GetExp(int exp)
    {
        this.exp += exp;

        while (this.exp >= nextExp)
        {
            // 초과분 넘기기
            this.exp = this.exp - nextExp;
            level++;
            hpValue.RuntimeMaxValue += 2;
            hpValue.RuntimeValue += 2;
            nextExp = 20 + (level + 2) * (level + 2);
        }

        playerValueChangeEvent.Occurred();

        GetExpEvent?.Invoke(level, this.exp, nextExp);
    }

    public void AddPassive(List<PassiveSO> passiveList)
    {
        foreach (PassiveSO so in passiveList) // 패시브 목록 추가
        {
            Passive passive = new Passive();
            so.Init(passive);
            buffCon.AddPassiveBuff(passive);
        }
    }
}