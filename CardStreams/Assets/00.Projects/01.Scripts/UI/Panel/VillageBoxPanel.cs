using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBoxPanel : MonoBehaviour
{
    // 마을에서 얻는 보상.. 돈 / 피 / 방어도/ 경험치 / 패시브 / 카드 같은거 싹 까면서(랜덤위치로 날라가서 잠시대기하고 도착지로 다시가기가능?)

    [Header("마을에서 획득하는 것들")]
    public IntValue goldValue; // 돈
    public IntValue hpValue; // 체력
    public IntValue shieldValue; // 방어막
    public IntValue startExpValue; // 경험치
    public PassiveListSO passiveListSO; // 패시브
    public BuildListSO merchantBuildListSO; // 카드 - 건물
    public SpecialCardListSO merchantCardListSO; // 카드 - 특수카드

    [Header("각 카드의 스프라이트")]
    [SerializeField] Sprite goldSprite;
    [SerializeField] Sprite hpSprite;
    [SerializeField] Sprite shieldSprite;
    [SerializeField] Sprite expSprite;
    [SerializeField] Sprite passiveSprite; // 이건 떼와야하는거아닌가?
    [SerializeField] Sprite buildOrSpeicalSprite; // 얘도 떼와야할듯 (각자의 listSO에서)

    [Header("카드를 보낼 위치들")]
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
        Debug.Log("테스트");
        GetHp();
    }

    // 아 클래스의 냄새가 너무 진하게나는데(중복 많음)    
    public void OnClickBoxOpen() // 상자깡 시작!
    {
        CreateCard(goldSprite, goldTrm, GetGold);
        CreateCard(hpSprite, hpTrm, GetHp);
        CreateCard(expSprite, expTrm, GetExp);
        CreateCard(shieldSprite, shieldTrm, GetShield);
        CreateCard(passiveSprite, passiveTrms[0], GetPassive); // 여기 패시브좀 나중에 다양 
        CreateCard(buildOrSpeicalSprite, bagTrm, GetCard);
    }

    /// <summary> 베지어카드를 만드는 함수 </summary>
    /// <param name="sprite">카드의 아이콘</param>
    /// <param name="callback">카드가 사라졌을때 함수 실행</param>
    /// <param name="targetTrm">카드가 도착할 위치</param>
    private void CreateCard(Sprite sprite, Transform targetTrm, Action callback) // 상자에서 카드가 나옴, 목적지 도착하면 callback 실행
    {
        EffectManager.Instance.GetBezierCardEffect(transform.position, sprite, targetTrm, callback);
    }

    private void GetRandomCards() // 상인의 랜덤 카드 획득
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
        // 9개는 돈x 마지막1개에 돈 몰아줘서 RuntimeValue 세팅
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
        // 나중에 구현좀ㅋㅋ hp처럼
    }

    private void GetExp() // 경험치 카드 획득
    {
        // 여기도 10개 날려서 9개는 아무것도아니고 마지막1개는 경험치 쫙얻는거로
        GameManager.Instance.player.GetExp(startExpValue.RuntimeValue);
    }

    private void GetPassive()
    {
        // 여기도 3개보내서 2개는 x 마지막1개에 3개다세팅
        GameManager.Instance.player.AddPassive(passiveListSO.passiveList);
    }

    private void GetCard()
    {
        // 나중에구현 상인 카드
    }

    private void OnDestroy()
    {
        merchantBuildListSO.buildList.Clear();
        merchantCardListSO.specialCardList.Clear();

        // 패시브
        foreach (PassiveSO so in passiveListSO.passiveList)
        {
            so.currentLevel = 1;
        }

        passiveListSO.passiveList.Clear();
    }
}
