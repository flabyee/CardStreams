using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UseTiming
{
    BeforePotion,
    AfterPotion,
    BeforeSword,
    AfterSword,
    BeforeShield,
    AfterShield,
    MoveEnd
}

public class Buff
{
    public int id;
    public UseTiming timing; // ����Ÿ�̹�
    public int remainTime; // ���ӽð�
    public string buffName; // �����̸�
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
}
