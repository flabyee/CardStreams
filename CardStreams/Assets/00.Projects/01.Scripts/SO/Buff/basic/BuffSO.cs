using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSO : ScriptableObject
{
    public int id;
    public UseTiming timing; // 쓰는타이밍
    public int remainTime; // 지속시간
    public string buffName;
    public Sprite buffIcon;

    // 원래는 쓰는자식한테만 대충 넣어놔야함

    // public abstract int BuffOn(int prevDamage); //  버프 들어갈때 추가, 카드 사용될때 호출

    public void Init(Buff buff)
    {
        buff.id = id;
        buff.timing = timing;
        buff.buffName = buffName;
        buff.buffIcon = buffIcon;
        buff.remainTime = remainTime;

        buff.OnBuff += UseBuff; // 액션
    }

    public abstract void UseBuff(int prevDamage); // 버프 특수기능들
}