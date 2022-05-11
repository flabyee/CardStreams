using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSO : ScriptableObject
{
    public int id;
    public string buffName;
    public Sprite buffIcon;

    public bool isOneWork; // �ѹ����� ������� ����?

    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;

    public abstract int BuffOn(int prevDamage); //  ���� ���� �߰�, ī�� ���ɶ� ȣ��

    public bool BuffOff() // ���� �̴�� ������? ������ true
    {
        // ���������°� ���߿�����ٰ� true/false ó��

        return isOneWork;
    }
}