using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private RectTransform rectTrm;
    private BuffController buffCon;

    [SerializeField] EventSO crystalChangeEvent;
    public EventSO playerValueChangeEvent;
    public VillageBuffListSO buffListSO;

    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;
    public IntValue goldValue;

    public bool isAlive { get; private set; }

    // ���� ����
    private int level = 1;
    private int exp;
    private int nextExp = 30;
    public Action<int, int, int> GetExpEvent;

    [Header("Debug")]
    public DebugBoolSO isDontDie;

    protected virtual void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        buffCon = GetComponent<BuffController>();

        // IntValue Init : VillagePlayer�� �Űܼ� �������� ��ġ�ٲܼ��ְ�
        //hpValue.RuntimeValue = hpValue.InitialMaxValue;
        //hpValue.RuntimeMaxValue = hpValue.InitialMaxValue;
        if (hpValue.RuntimeMaxValue <= 0)
        {
            hpValue.RuntimeMaxValue = hpValue.InitialMaxValue;
        }
        hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
        swordValue.RuntimeValue = 0;
        shieldValue.RuntimeValue = 0;
    }


    void Start()
    {
        foreach (BuffSO so in buffListSO.buffList)
        {
            Buff buff = new Buff();
            so.Init(buff); // SO�� ������ Buff�� �ʱ�ȭ����
            buffCon.AddBuff(buff);
        }
        playerValueChangeEvent.Occurred();

        isAlive = true;
    }

    public void CheckPlayerAlive() // �÷��̾� ���������� �˻��ϴ� �޼ҵ� | PlayerValueChanged�� ������ ó�� Init�� �ɷ��� �ȵ�
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
                Debug.LogError("ī�� Ÿ���� null");
                break;
        }

        playerValueChangeEvent.Occurred();
        

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardPower);

    }

    public bool OnBoss(int damage, out int sword)
    {
        int currentMonsterValue = damage;

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

        // ������ ����� 0 �̻��� ����
        if (currentMonsterValue > 0)
        {
            // Į ��� �˻�
            if (currentMonsterValue > 0 && swordValue.RuntimeValue > 0) // ���� ���ݷ��� 0 �̻��̰� Į�� ������
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


            // Į0 (�ְ� ����)
            swordValue.RuntimeValue = 0;

            // ���� ������
            shieldValue.RuntimeValue = leftShieldValue;

            // ���������� ����
            currentMonsterValue = Mathf.Clamp(currentMonsterValue, 0, 99);
            hpValue.RuntimeValue -= currentMonsterValue;
            playerValueChangeEvent.Occurred();
        }

        // ųī��Ʈ, ũ����Ż ��
        Crystal.crystalAmount += GameManager.Instance.mineLevel;
        crystalChangeEvent?.Occurred();

        GetExp(cardPower.originValue);
    }

    private void GetExp(int exp)
    {
        this.exp += exp;

        if(this.exp >= nextExp)
        {
            // �ʰ��� �ѱ��
            this.exp = this.exp - nextExp;
            level++;
            hpValue.RuntimeMaxValue += 2;
            nextExp = 20 + (level + 2) * (level + 2);

            playerValueChangeEvent.Occurred();
        }

        GetExpEvent?.Invoke(level, this.exp, nextExp);
    }
}