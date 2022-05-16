using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    private readonly List<Buff> buffList = new List<Buff>();

    public void AddBuff(Buff buff)
    {
        // 중복검사, if (버프가 중복) return
        if (!CanAddBuff(buff)) return;

        buffList.Add(buff);
    }

    public void RemoveTimeOutBuff() // 지속시간 0된 버프들을 검사하고 삭제합니다.
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

    public void UseBuffs(UseTiming timing, int prevDamage) // 카드나 플레이어에 들어가있는 버프들를 실행
    {
        foreach (var buff in buffList)
        {
            Debug.Log("Use Buff");

            if (timing == buff.timing)
            {
                Debug.Log("Use Buff Timing");
                buff.UseBuff(prevDamage);
                buff.ReduceBuffTime();
            }
        }

        RemoveTimeOutBuff(); // 다 쓰고 
    }

    public bool CanAddBuff(Buff buff) // 중복이면 False
    {
        if(buffList.Contains(buff))
        {
            return false;
        }

        return true;
    }
}
