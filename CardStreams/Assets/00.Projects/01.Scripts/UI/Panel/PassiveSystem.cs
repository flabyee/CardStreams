using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSystem : MonoBehaviour
{
    [SerializeField] PassiveSlot[] slots = new PassiveSlot[3];
    [SerializeField] PassiveListSO passiveListSO;

    [SerializeField] List<PassiveSO> registerSOList = new List<PassiveSO>();
    private int registerPassiveCount;

    public void AddPassive(PassiveSO so)
    {
        // �нú갡 0���϶� �˻� ���ϵ���, 3���϶� 0 1 2 3��������
        for (int i = 0; i < registerPassiveCount; i++)
        {
            var passive = registerSOList[i];
            if (passive == so) // �̹� �нú�â�� �нú갡 �ִٸ�
            {
                if (passive.currentLevel < 3) // 3 �̸��϶��� ��ų ������
                {
                    passive.LevelUp();
                    slots[i].LevelUpdate(passive.currentLevel);
                }
                return;
            }
        }

        if (registerPassiveCount >= 3) return; // �ߺ��� �ƴѵ� �нú갡 3���� = �ڸ� ����

        // �ߺ��� �ƴ϶�� �нú�â�� �߰� (�ߺ��� �нú�� ������ �ɷ���)
        slots[registerPassiveCount].Init(so.buffIcon);
        registerSOList.Add(so);
        registerPassiveCount++;
    }

    public void AddToPlayerBuffList() // �нú� ����� �÷��̾� ������Ͽ� �߰��ϱ�
    {
        passiveListSO.dontDestroyPassiveList.AddRange(registerSOList);
    }
}
