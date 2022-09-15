using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour 
{
    private readonly List<Buff> buffList = new List<Buff>();
    [SerializeField] PassiveDisplayPanel passiveDisplayPanel;

    public void AddBuff(Buff buff)
    {
        // �ߺ��˻�, if (������ �ߺ�) return
        if (!CanAddBuff(buff)) return;

        buffList.Add(buff);
    }

    public void AddPassiveBuff(Buff buff)
    {
        // �ߺ��˻�, if (������ �ߺ�) return
        if (!CanAddBuff(buff)) return;

        // � ���� �߰����� ������ ���ʻ�ܿ� ǥ��
        passiveDisplayPanel.SetPassive(buff.buffIcon, buff.buffName);

        buffList.Add(buff);
    }

    public void RemoveTimeOutBuff() // ���ӽð� 0�� �������� �˻��ϰ� �����մϴ�.
    {
        var willRemove = new HashSet<Buff>();

        foreach (var buff in buffList)
        {
            if(buff.IsTimeOutBuff())
            {
                willRemove.Add(buff);
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
