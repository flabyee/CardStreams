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

    // ���� ����
    private int level = 1;
    private int exp;
    private int nextExp = 30;
    public Action<int, int, int> GetExpEvent;

    // �̼� �ý����� ����


    [Header("Debug")]
    public DebugBoolSO isDontDie;



    protected virtual void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        buffCon = GetComponent<BuffController>();

        hpValue.RuntimeMaxValue = hpValue.InitialMaxValue;
        hpValue.RuntimeValue = hpValue.RuntimeMaxValue;

        swordValue.RuntimeValue = 0;
        shieldValue.RuntimeValue = 0;
    }


    void Start()
    {
        isAlive = true;
    }

    public void CheckPlayerAlive() // �÷��̾� ���������� �˻��ϴ� �޼ҵ� | PlayerValueChanged�� ������ ó�� Init�� �ɷ��� �ȵ�
    {
        if (isDontDie.b == true)
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
                OnPotion(field.cardPower as BasicCard);
                break;

            case BasicType.Sword:
                OnSword(field.cardPower as BasicCard);
                break;

            case BasicType.Sheild:
                OnShield(field.cardPower as BasicCard);
                break;

            case BasicType.Monster:
                OnMonster(field.cardPower as BasicCard);
                break;

            default:
                Debug.LogError("ī�� Ÿ���� null");
                break;
        }

        playerValueChangeEvent.Occurred();

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower);

        // �̼�
        MissionObserverManager.instance.OnBasicCard?.Invoke(field.cardPower);
    }

    public bool OnBoss(int currentMonsterValue, out int sword)
    {
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

        // Į ����ؼ� �������� �߰� ����
        sword = swordValue.RuntimeValue;
        swordValue.RuntimeValue = 0;

        // ���� ������
        shieldValue.RuntimeValue = leftShieldValue;

        // ���������� ����
        currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, 99);
        hpValue.RuntimeValue -= currentMonsterValue;
        playerValueChangeEvent.Occurred();


        return hpValue.RuntimeValue <= 0;
    }

    private void AddFieldBuff(List<Buff> buffList) // �ʵ��� �������� ������Ʈ�ѷ��� �߰�
    {
        foreach (Buff buff in buffList)
        {
            buffCon.AddBuff(buff);
        }
    }

    private void OnPotion(BasicCard fieldCardPower) // ü����������
    {
        if (fieldCardPower.value > 0)
        {
            AddFieldBuff(fieldCardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetPotion, fieldCardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�

            hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + fieldCardPower.value, 0, hpValue.RuntimeMaxValue);

            buffCon.UseBuffs(UseTiming.AfterGetPotion, fieldCardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�
        }
    }

    private void OnSword(BasicCard fieldCardPower)
    {
        if (fieldCardPower.value > 0)
        {
            AddFieldBuff(fieldCardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetSword, fieldCardPower.value);

            swordValue.RuntimeValue = fieldCardPower.value;

            buffCon.UseBuffs(UseTiming.AfterGetSword, fieldCardPower.value);
        }
    }

    private void OnShield(BasicCard fieldCardPower)
    {
        if (fieldCardPower.value > 0)
        {
            AddFieldBuff(fieldCardPower.buffList);

            buffCon.UseBuffs(UseTiming.GetShield, fieldCardPower.value);

            int addShieldValue = fieldCardPower.value + shieldValue.RuntimeValue;

            shieldValue.RuntimeValue = fieldCardPower.value;

            buffCon.UseBuffs(UseTiming.AfterGetShield, addShieldValue);
        }
    }

    private void OnMonster(BasicCard fieldCardPower)
    {
        int currentMonsterValue = fieldCardPower.value;

        // ������ ����� 0 �̻��� ����
        if (currentMonsterValue > 0)
        {
            // Į ��� �˻�
            if (currentMonsterValue > 0 && swordValue.RuntimeValue > 0) // ���� ���ݷ��� 0 �̻��̰� Į�� ������
            {
                buffCon.UseBuffs(UseTiming.BeforeSword, fieldCardPower.value); // 0�� �ƹ��ǹ̵����� int? ���߿�

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
                    MissionObserverManager.instance.OnShield?.Invoke(shieldValue.RuntimeValue);

                    currentMonsterValue -= shieldValue.RuntimeValue; // ���� -= ����
                    leftShieldValue = 0; // ���� 0
                }
                else // ���а���ũ��
                {
                    MissionObserverManager.instance.OnShield?.Invoke(currentMonsterValue);

                    leftShieldValue -= currentMonsterValue; // ���� -= ����
                    currentMonsterValue = 0; // ���� 0
                }

                buffCon.UseBuffs(UseTiming.AfterShield, currentMonsterValue);
            }


            // Į0 (�ְ� ����)
            swordValue.RuntimeValue = 0;

            // ���� ������
            shieldValue.RuntimeValue = leftShieldValue;

            // ���������� ����
            currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, 99);
            hpValue.RuntimeValue -= currentMonsterValue;
            playerValueChangeEvent.Occurred();
        }

        // ųī��Ʈ, �� �� (ũ����Ż�� ���� �Ϸ�ø��� 5��, ���ӸŴ����� �̻簨)
        ResourceManager.Instance.AddResource(ResourceType.prestige, 1);

        GetExpBezier(fieldCardPower.originValue);
    }

    public void GetExpBezier(int exp)
    {
        for (int i = 0; i < exp; i++)
        {
            EffectManager.Instance.CreateBezierEffect(transform.position, ConstManager.Instance.expSprites[UnityEngine.Random.Range(0, ConstManager.Instance.expSprites.Length)],
                TargetType.Exp, () => { GetExp(1); }, 1.1f, 1f, 2f, false, 1f);
        }
    }
    public void GetExpBezier(int exp, Vector3 pos)
    {
        for (int i = 0; i < exp; i++)
        {
            EffectManager.Instance.CreateBezierEffect(pos, ConstManager.Instance.expSprites[UnityEngine.Random.Range(0, ConstManager.Instance.expSprites.Length)],
                TargetType.Exp, () => { GetExp(1); }, 1.1f, 1f, 2f, false, 1f);
        }
    }

    public void GetExp(int exp)
    {
        this.exp += exp;

        while (this.exp >= nextExp)
        {
            // �ʰ��� �ѱ��
            this.exp -= nextExp;
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
        foreach (PassiveSO so in passiveList) // �нú� ��� �߰�
        {
            Passive passive = new Passive();
            so.Init(passive);
            buffCon.AddPassiveBuff(passive);
        }
    }
}