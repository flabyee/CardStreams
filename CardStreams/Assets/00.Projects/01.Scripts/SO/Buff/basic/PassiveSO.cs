using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSO : BuffSO
{
    public int currentLevel = 0; // SO긴 한데 외부의 값으로 수치가 바뀌는 변수입니다. 레벨 어떻게하지
    public override void UseBuff(int fieldValue)
    {
        Debug.Log("기본 패시브를 발동합니다.");
    }

    public virtual void Init(Passive passive)
    {
        base.Init(passive);
        passive.currentLevel = currentLevel;
    }

    public void LevelUp()
    {
        currentLevel++;
    }
}