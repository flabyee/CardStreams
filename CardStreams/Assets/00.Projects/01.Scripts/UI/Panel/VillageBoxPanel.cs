using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBoxPanel : Panel
{
    public VillageRewardListSO rewardListSO;
    public PassiveListSO passiveListSO; // 패시브

    [Header("카드를 보낼 위치들")]
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

    public void OnClickBoxOpen() // 상자깡 시작!
    {
        foreach (var reward in rewardListSO.rewardList)
        {
            CreateRewardCard(reward); // 최대체력 늘려주는거 근데 얘가 카드임. 카드가 체력UI에 닿으면 사라지면서 체력+
        }

        AfterOpen();
    }

    private void CreateRewardCard(VillageRewardSO so) // 상자에서 카드가 나옴, 목적지 도착하면 callback 실행
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
                Debug.Log("목표 Transform이 없어서 보상획득불가");
                return;
        }

        EffectManager.Instance.CreateBezierEffect(transform.position, so.rewardSprite, targetTrm, so.GetReward);
    }

    private void AfterOpen()
    {
        rewardListSO.rewardList.Clear(); // 보상 다먹었으니 초기화
        GameManager.Instance.canNextLoop = true;
        GameManager.Instance.blurController.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        rewardListSO.rewardList.Clear(); // 수동으로 비워줘야함(끌땐무조건)

        // 패시브
        foreach (PassiveSO so in passiveListSO.dontDestroyPassiveList)
        {
            so.currentLevel = 1;
        }

        passiveListSO.dontDestroyPassiveList.Clear();
    }
}
