using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSO : ScriptableObject
{
    public int id;
    public UseTiming timing; // ����Ÿ�̹�
    public int remainTime; // ���ӽð�
    public string buffName;
    public Sprite buffIcon;

    // ������ �����ڽ����׸� ���� �־������

    // public abstract int BuffOn(int prevDamage); //  ���� ���� �߰�, ī�� ���ɶ� ȣ��

    public void Init(Buff buff)
    {
        buff.id = id;
        buff.timing = timing;
        buff.buffName = buffName;
        buff.buffIcon = buffIcon;
        buff.remainTime = remainTime;

        buff.OnBuff += UseBuff; // �׼�
    }

    public abstract void UseBuff(int prevDamage); // ���� Ư����ɵ�
}