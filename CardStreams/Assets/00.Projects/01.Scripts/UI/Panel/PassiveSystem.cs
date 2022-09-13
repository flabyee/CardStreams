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
        // �нú갡 0���϶� �˻� ���ϵ���, 3���϶� 0 1 2 3��������
        for (int i = 0; i < registerPassiveCount; i++)
        {
            if(registerSOList[i] == so)
            {
                soLevelList[i]++;
                slots[i].LevelUpdate(soLevelList[i]);
                return;
            }
        }

        // �ߺ��� �ƴ϶�� �нú�â�� �߰� (�ߺ��� �нú�� ������ �ɷ���)
        slots[registerPassiveCount].Init(so.buffIcon);
        registerSOList.Add(so);
        soLevelList.Add(1);
        registerPassiveCount++;
    }

    public void AddToPlayerBuffList() // �нú� ����� �÷��̾� ������Ͽ� �߰��ϱ�
    {
        // ���߿� ������ ����ؼ� ������ �нú���� ������ ��
        buffListSO.buffList.AddRange(registerSOList);
    }
}
