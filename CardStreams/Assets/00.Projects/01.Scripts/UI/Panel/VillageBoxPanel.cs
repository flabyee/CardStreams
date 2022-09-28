using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBoxPanel : MonoBehaviour
{
    // �������� ��� ����.. �� / �� / ��/ ����ġ / �нú� / ī�� ������ �� ��鼭(������ġ�� ���󰡼� ��ô���ϰ� �������� �ٽð��Ⱑ��?)

    [Header("��ü��")]
    public VillageRewardListSO rewardListSO;

    //[Header("�������� ȹ���ϴ� �͵�")]
    //public IntValue goldValue; // ��
    //public IntValue hpValue; // ü��
    //public IntValue shieldValue; // ��
    //public IntValue startExpValue; // ����ġ
    public PassiveListSO passiveListSO; // �нú�
    public BuildListSO merchantBuildListSO; // ī�� - �ǹ�
    public SpecialCardListSO merchantCardListSO; // ī�� - Ư��ī��

    //[Header("�� ī���� ��������Ʈ")]
    //[SerializeField] Sprite goldSprite;
    //[SerializeField] Sprite hpSprite;
    //[SerializeField] Sprite shieldSprite;
    //[SerializeField] Sprite expSprite;
    //[SerializeField] Sprite passiveSprite; // �̰� ���;��ϴ°žƴѰ�?
    //[SerializeField] Sprite buildOrSpeicalSprite; // �굵 ���;��ҵ� (������ listSO����)

    [Header("ī�带 ���� ��ġ��")]
    [SerializeField] Transform goldTrm;
    [SerializeField] Transform hpTrm;
    [SerializeField] Transform shieldTrm;
    [SerializeField] Transform expTrm;
    [SerializeField] Transform passiveTrm;
    [SerializeField] Transform bagTrm;

    [Header("Event")]
    [SerializeField] EventSO playerValueChangeEvent;
    // [SerializeField] EventSO goldValueChangeEvent;

    // public List<VillageRewardSO> rewardList = new List<VillageRewardSO>();

    private void Start()
    {
        //Debug.Log("�׽�Ʈ");
        //foreach (var reward in rewardListSO.rewardList)
        //{
        //    reward.GetReward(); // ���� �����Ͽ� �ִ��� ��μ���
        //}
        //playerValueChangeEvent?.Occurred();

        // GetHp();
    }

    // �� Ŭ������ ������ �ʹ� ���ϰԳ��µ�(�ߺ� ����)    
    public void OnClickBoxOpen() // ���ڱ� ����!
    {
        foreach (var reward in rewardListSO.rewardList)
        {
            CreateRewardCard(reward);
            //reward.GetReward(); // ���� �����Ͽ� �ִ��� ��μ���
        }
        playerValueChangeEvent?.Occurred();

        rewardListSO.rewardList.Clear(); // ���� �ٸԾ����� �ʱ�ȭ

        // ī�嵵���ؼ� �Ǵþ�� +13 +13 �̷��͵� ǥ�õ��������ٴ�. ���������� �����

        //CreateCard(goldSprite, goldTrm, GetGold);
        //CreateCard(hpSprite, hpTrm, GetHp);
        //CreateCard(expSprite, expTrm, GetExp);
        //CreateCard(shieldSprite, shieldTrm, GetShield);
        //CreateCard(passiveSprite, passiveTrms[0], GetPassive); // ���� �нú��� ���߿� �پ� 
        //CreateCard(buildOrSpeicalSprite, bagTrm, GetCard);
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

    //private void GetRandomCards() // ������ ���� ī�� ȹ��
    //{
    //    foreach (BuildSO so in merchantBuildListSO.buildList)
    //    {
    //        GameManager.Instance.handleController.AddBuild(so.id);
    //    }

    //    foreach (SpecialCardSO so in merchantCardListSO.specialCardList)
    //    {
    //        GameManager.Instance.handleController.AddSpecial(so.id);
    //    }
    //}

    //private void GetGold()
    //{
    //    // 9���� ��x ������1���� �� �����༭ RuntimeValue ����
    //    goldValue.RuntimeValue = 20 + goldValue.RuntimeMaxValue;
    //    goldCValuehangeEvent.Occurred();
    //}

    //private void GetHp()
    //{
    //    hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
    //    playerValueChangeEvent.Occurred();
    //}

    //private void GetShield()
    //{
    //    // ���߿� ���������� hpó��
    //}

    //private void GetExp() // ����ġ ī�� ȹ��
    //{
    //    // ���⵵ 10�� ������ 9���� �ƹ��͵��ƴϰ� ������1���� ����ġ �Ӿ�°ŷ�
    //    GameManager.Instance.player.GetExp(startExpValue.RuntimeValue);
    //}

    //private void GetPassive()
    //{
    //    // ���⵵ 3�������� 2���� x ������1���� 3���ټ���
    //    GameManager.Instance.player.AddPassive(passiveListSO.passiveList);
    //}

    private void GetCard()
    {
        // ���߿����� ���� ī��
    }

    private void OnDestroy()
    {
        rewardListSO.rewardList.Clear(); // �������� ��������(����������)

        merchantBuildListSO.buildList.Clear();
        merchantCardListSO.specialCardList.Clear();

        // �нú�
        foreach (PassiveSO so in passiveListSO.passiveList)
        {
            so.currentLevel = 1;
        }

        passiveListSO.passiveList.Clear();
    }
}
