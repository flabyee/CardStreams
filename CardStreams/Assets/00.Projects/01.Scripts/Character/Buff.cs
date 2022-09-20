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
    public UseTiming timing; // 쓰는타이밍
    public int remainTime; // 지속시간
    public string buffName; // 버프이름
    public string buffTooltip; // 버프내용
    public Sprite buffIcon; // 버프아이콘

    public Action<int> OnBuff; // 인자 : 이전 데미지

    public void BuffStart()
    {
        // 버프를 켜줍니다
    }

    public void BuffEnd()
    {
        // 버프를 꺼줍니다
    }

    public bool IsTimeOutBuff() // 버프 시간이 남았으면 True
    {
        if(remainTime <= 0) // 시간이 다달았으면 True
        {
            return true;
        }

        return false;
    }

    public void ReduceBuffTime() // 버프 지속시간을 -1 합니다.
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
