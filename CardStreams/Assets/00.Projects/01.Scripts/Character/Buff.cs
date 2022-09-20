using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UseTiming
{
    GetPotion,
    AfterGetPotion,
    GetSword,
    AfterGetSword,
    GetShield,
    AfterGetShield,
    BeforeSword,
    AfterSword,
    BeforeShield,
    AfterShield,
    AfterMove,
}

public class Buff
{
    public int id;
    public UseTiming timing; // ����Ÿ�̹�
    public int remainTime; // ���ӽð�
    public string buffName; // �����̸�
    public string buffTooltip; // ��������
    public Sprite buffIcon; // ����������

    public Action<int> OnBuff; // ���� : ���� ������

    public void BuffStart()
    {
        // ������ ���ݴϴ�
    }

    public void BuffEnd()
    {
        // ������ ���ݴϴ�
    }

    public bool IsTimeOutBuff() // ���� �ð��� �������� True
    {
        if(remainTime <= 0) // �ð��� �ٴ޾����� True
        {
            return true;
        }

        return false;
    }

    public void ReduceBuffTime() // ���� ���ӽð��� -1 �մϴ�.
    {
        remainTime -= 1;
    }

    public void UseBuff(int prevDamage)
    {
        OnBuff(prevDamage);
    }

    public Buff GetCopyBuff()
    {
        Buff copyBuff = new Buff();

        copyBuff.id = id;
        copyBuff.timing = timing;
        copyBuff.remainTime = remainTime;
        copyBuff.buffName = buffName;
        copyBuff.buffIcon = buffIcon;

        copyBuff.OnBuff += OnBuff;

        return copyBuff;
    }
}
