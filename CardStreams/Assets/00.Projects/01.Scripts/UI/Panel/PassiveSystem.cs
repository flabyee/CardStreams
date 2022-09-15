using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSystem : MonoBehaviour
{
    [SerializeField] PassiveSlot[] slots = new PassiveSlot[3];
    [SerializeField] PassiveListSO passiveListSO;

    private List<PassiveSO> registerSOList = new List<PassiveSO>();
    private int registerPassiveCount;

    public void AddPassive(PassiveSO so)
    {
        // 패시브가 0개일땐 검사 안하도록, 3개일땐 0 1 2 3번돌도록
        for (int i = 0; i < registerPassiveCount; i++)
        {
            var passive = registerSOList[i];
            if (passive == so) // 이미 패시브창에 패시브가 있다면
            {
                if (passive.currentLevel < 3) // 3 미만일때는 스킬 레벨업
                {
                    passive.LevelUp();
                    slots[i].LevelUpdate(passive.currentLevel);
                }
                return;
            }
        }

        // 중복이 아니라면 패시브창에 추가 (중복된 패시브면 위에서 걸러짐)
        slots[registerPassiveCount].Init(so.buffIcon);
        registerSOList.Add(so);
        registerPassiveCount++;
    }

    public void AddToPlayerBuffList() // 패시브 목록을 플레이어 버프목록에 추가하기
    {
        // 나중엔 레벨에 비례해서 좋아진 패시브들을 보내야 함
        passiveListSO.passiveList.AddRange(registerSOList);
    }
}
