using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSO : ScriptableObject
{
    public int id;
    public string buffName;
    public Sprite buffIcon;

    public bool isOneWork; // 한번쓰면 사라지는 버프?

    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;

    public abstract int BuffOn(int prevDamage); //  버프 들어갈때 추가, 카드 사용될때 호출

    public bool BuffOff() // 버프 이대로 꺼지나? 꺼지면 true
    {
        // 여러번쓰는건 나중에여기다가 true/false 처리

        return isOneWork;
    }
}