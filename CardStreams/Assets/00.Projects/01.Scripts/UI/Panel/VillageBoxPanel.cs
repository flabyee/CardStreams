using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBoxPanel : Panel
{
    public VillageRewardListSO rewardListSO;
    public PassiveListSO passiveListSO; // �нú�

    [Header("ī�带 ���� ��ġ��")]
    [SerializeField] Transform goldTrm;
    [SerializeField] Transform hpTrm;
    [SerializeField] Transform shieldTrm;
    [SerializeField] Transform expTrm;
    [SerializeField] Transform passiveTrm;
    [SerializeField] Transform bagTrm;

    private void Start()
    {
        GameManager.Instance.blurController.SetActive(true);

        GameManager.Instance.player.AddPassive(passiveListSO.dontDestroyPassiveList);
    }

    public void OnClickBoxOpen() // ���ڱ� ����!
    {
        foreach (var reward in rewardListSO.rewardList)
        {
            CreateRewardCard(reward); // �ִ�ü�� �÷��ִ°� �ٵ� �갡 ī����. ī�尡 ü��UI�� ������ ������鼭 ü��+
        }

        AfterOpen();
    }

    private void CreateRewardCard(VillageRewardSO so) // ���ڿ��� ī�尡 ����, ������ �����ϸ� callback ����
    {
        Transform targetTrm = null;

        switch (so.rewardType)
        {
            case VillageRewardType.gold:
                targetTrm = goldTrm;
                break;
            case VillageRewardType.maxHp:
                targetTrm = hpTrm;
                break;
            case VillageRewardType.shield:
                targetTrm = shieldTrm;
                break;
            case VillageRewardType.exp:
                targetTrm = expTrm;
                break;
            case VillageRewardType.passive:
                targetTrm = passiveTrm;
                break;
            case VillageRewardType.card:
                targetTrm = bagTrm;
                break;
            default:
                Debug.Log("��ǥ Transform�� ��� ����ȹ��Ұ�");
                return;
        }

        EffectManager.Instance.CreateBezierEffect(transform.position, so.rewardSprite, targetTrm, so.GetReward);
    }

    private void AfterOpen()
    {
        rewardListSO.rewardList.Clear(); // ���� �ٸԾ����� �ʱ�ȭ
        GameManager.Instance.canNextLoop = true;
        GameManager.Instance.blurController.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        rewardListSO.rewardList.Clear(); // �������� ��������(����������)

        // �нú�
        foreach (PassiveSO so in passiveListSO.dontDestroyPassiveList)
        {
            so.currentLevel = 1;
        }

        passiveListSO.dontDestroyPassiveList.Clear();
    }
}
