using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour 
{
    private readonly List<Buff> buffList = new List<Buff>();
    [SerializeField] PassiveDisplayPanel passiveDisplayPanel;
    [SerializeField] PlayerBuffPanel playerBuffPanel;

    public void AddBuff(Buff buff)
    {
        // �ߺ��˻�, if (������ �ߺ�) return
        if (!CanAddBuff(buff)) return;

        buffList.Add(buff);
        playerBuffPanel.AddBuff(buff);
    }

    public void AddPassiveBuff(Passive passive)
    {
        // �ߺ��˻�, if (������ �ߺ�) return
        if (!CanAddBuff(passive)) return;

        // � ���� �߰����� ������ ���ʻ�ܿ� ǥ��
        passiveDisplayPanel.SetPassive(passive);

        buffList.Add(passive);
    }

    public void RemoveTimeOutBuff() // ���ӽð� 0�� �������� �˻��ϰ� �����մϴ�.
    {
        var willRemove = new HashSet<Buff>();

        foreach (var buff in buffList)
        {
            if(buff.IsTimeOutBuff())
            {
                willRemove.Add(buff);
                playerBuffPanel.RemoveBuff(buff);
            }
        }

         buffList.RemoveAll(willRemove.Contains);
    }

    public void UseBuffs(UseTiming timing, int prevDamage) // ī�峪 �÷��̾ ���ִ� �����鸦 ����
    {
        foreach (var buff in buffList)
        {
            //Debug.Log("Use Buff");

            if (timing == buff.timing)
            {
                //Debug.Log("Use Buff Timing");
                buff.UseBuff(prevDamage);
                buff.ReduceBuffTime();
            }
        }

        RemoveTimeOutBuff(); // �� ���� 
    }

    public bool CanAddBuff(Buff buff) // �ߺ��̸� False
    {
        if(buffList.Contains(buff))
        {
            return false;
        }

        return true;
    }

    public void RemoveAllBuff()
    {
        buffList.Clear();
    }

    public List<Buff> GetBuffList()
    {
        return buffList;
    }
}
