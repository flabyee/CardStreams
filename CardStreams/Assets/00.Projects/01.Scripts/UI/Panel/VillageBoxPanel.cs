using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBoxPanel : MonoBehaviour
{
    // �������� ��� ����.. �� / �� / ��/ ����ġ / �нú� / ī�� ������ �� ��鼭(������ġ�� ���󰡼� ��ô���ϰ� �������� �ٽð��Ⱑ��?)

    [Header("�������� ȹ���ϴ� �͵�")]
    public IntValue goldValue; // ��
    public IntValue hpValue; // ü��
    public IntValue shieldValue; // ��
    public IntValue startExpValue; // ����ġ
    public PassiveListSO passiveListSO; // �нú�
    public BuildListSO merchantBuildListSO; // ī�� - �ǹ�
    public SpecialCardListSO merchantCardListSO; // ī�� - Ư��ī��

    [Header("�� ī���� ��������Ʈ")]
    [SerializeField] Sprite goldSprite;
    [SerializeField] Sprite hpSprite;
    [SerializeField] Sprite shieldSprite;
    [SerializeField] Sprite expSprite;
    [SerializeField] Sprite passiveSprite; // �̰� ���;��ϴ°žƴѰ�?
    [SerializeField] Sprite buildOrSpeicalSprite; // �굵 ���;��ҵ� (������ listSO����)

    [Header("ī�带 ���� ��ġ��")]
    [SerializeField] Transform goldTrm;
    [SerializeField] Transform hpTrm;
    [SerializeField] Transform shieldTrm;
    [SerializeField] Transform expTrm;
    [SerializeField] Transform[] passiveTrms;
    [SerializeField] Transform bagTrm;

    [Header("Event")]
    [SerializeField] EventSO playerValueChangeEvent;
    [SerializeField] EventSO goldCValuehangeEvent;

    private void Start()
    {
        Debug.Log("�׽�Ʈ");
        GetHp();
    }

    // �� Ŭ������ ������ �ʹ� ���ϰԳ��µ�(�ߺ� ����)    
    public void OnClickBoxOpen() // ���ڱ� ����!
    {
        CreateCard(goldSprite, goldTrm, GetGold);
        CreateCard(hpSprite, hpTrm, GetHp);
        CreateCard(expSprite, expTrm, GetExp);
        CreateCard(shieldSprite, shieldTrm, GetShield);
        CreateCard(passiveSprite, passiveTrms[0], GetPassive); // ���� �нú��� ���߿� �پ� 
        CreateCard(buildOrSpeicalSprite, bagTrm, GetCard);
    }

    /// <summary> ������ī�带 ����� �Լ� </summary>
    /// <param name="sprite">ī���� ������</param>
    /// <param name="callback">ī�尡 ��������� �Լ� ����</param>
    /// <param name="targetTrm">ī�尡 ������ ��ġ</param>
    private void CreateCard(Sprite sprite, Transform targetTrm, Action callback) // ���ڿ��� ī�尡 ����, ������ �����ϸ� callback ����
    {
        EffectManager.Instance.GetBezierCardEffect(transform.position, sprite, targetTrm, callback);
    }

    private void GetRandomCards() // ������ ���� ī�� ȹ��
    {
        foreach (BuildSO so in merchantBuildListSO.buildList)
        {
            GameManager.Instance.handleController.AddBuild(so.id);
        }

        foreach (SpecialCardSO so in merchantCardListSO.specialCardList)
        {
            GameManager.Instance.handleController.AddSpecial(so.id);
        }
    }

    private void GetGold()
    {
        // 9���� ��x ������1���� �� �����༭ RuntimeValue ����
        goldValue.RuntimeValue = 20 + goldValue.RuntimeMaxValue;
        goldCValuehangeEvent.Occurred();
    }

    private void GetHp()
    {
        hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
        playerValueChangeEvent.Occurred();
    }

    private void GetShield()
    {
        // ���߿� ���������� hpó��
    }

    private void GetExp() // ����ġ ī�� ȹ��
    {
        // ���⵵ 10�� ������ 9���� �ƹ��͵��ƴϰ� ������1���� ����ġ �Ӿ�°ŷ�
        GameManager.Instance.player.GetExp(startExpValue.RuntimeValue);
    }

    private void GetPassive()
    {
        // ���⵵ 3�������� 2���� x ������1���� 3���ټ���
        GameManager.Instance.player.AddPassive(passiveListSO.passiveList);
    }

    private void GetCard()
    {
        // ���߿����� ���� ī��
    }

    private void OnDestroy()
    {
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
