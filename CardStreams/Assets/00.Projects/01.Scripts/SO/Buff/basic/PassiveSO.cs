using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSO : BuffSO
{
    public int currentLevel = 0; // SO�� �ѵ� �ܺ��� ������ ��ġ�� �ٲ�� �����Դϴ�. ���� �������
    public override void UseBuff(int fieldValue)
    {
        Debug.Log("�⺻ �нú긦 �ߵ��մϴ�.");
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