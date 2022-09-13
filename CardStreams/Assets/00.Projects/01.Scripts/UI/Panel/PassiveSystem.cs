using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSystem : MonoBehaviour
{
    [SerializeField] PassiveSlot[] slots = new PassiveSlot[3];
    [SerializeField] VillageBuffListSO buffListSO;

    private int registerPassiveCount;

    private List<BuffSO> registerSOList = new List<BuffSO>();
    private List<int> soLevelList = new List<int>();

    public void AddPassive(BuffSO so)
    {
        // 패시브가 0개일땐 검사 안하도록, 3개일땐 0 1 2 3번돌도록
        for (int i = 0; i < registerPassiveCount; i++)
        {
            if(registerSOList[i] == so)
            {
                soLevelList[i]++;
                slots[i].LevelUpdate(soLevelList[i]);
                return;
            }
        }

        // 중복이 아니라면 패시브창에 추가 (중복된 패시브면 위에서 걸러짐)
        slots[registerPassiveCount].Init(so.buffIcon);
        registerSOList.Add(so);
        soLevelList.Add(1);
        registerPassiveCount++;
    }

    public void AddToPlayerBuffList() // 패시브 목록을 플레이어 버프목록에 추가하기
    {
        // 나중엔 레벨에 비례해서 좋아진 패시브들을 보내야 함
        buffListSO.buffList.AddRange(registerSOList);
    }
}
