using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public enum TargetType // 보상카드가 날아갈 위치
{
    Bag,
    GoldUI,
    HPUI,
    Exp,
}

public class RewardCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject cover; // 카드 누르기전 앞면

    [Header("공통 카드 속성")]
    [SerializeField] Image rewardImage; // 카드의 이미지
    [SerializeField] TextMeshProUGUI rewardNameText; // 카드의 이름
    [SerializeField] TextMeshProUGUI rewardDescriptionText; // 카드의 설명


    [Header("테스트 : 단순 카드일때만 쓰는 멤버")]
    [SerializeField] int getGoldAmount = 0;
    [SerializeField] bool getHeal = false;


    [Header("단순 스텟 관련")]
    [SerializeField] Sprite healSprite;
    [SerializeField] Sprite goldSprite;

    [Header("단순 스텟 조절목적 SO")]
    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;
    [SerializeField] EventSO goldValueChanged;
    [SerializeField] EventSO playerValueChanged;

    public bool isGet = false; // 카드의 보상을 얻었나요?
    
    private float reverseTime = 1f; // 카드가 뒤집어지는 시간
    private bool isCompleteReverse = false; // 카드가 뒤집어져 보상 앞면이 노출되었나요?


    private SpecialCardSO cardSO;
    
    private void DefaultInit(float cardReverseTime) // 카드 뽑을때 해주기(시작시에잠깐)
    {
        cover.SetActive(true);
        gameObject.SetActive(true);
        reverseTime = cardReverseTime;
        isGet = false;
        isCompleteReverse = false;
    }

    public void CardInit(SpecialCardSO so, float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;
        rewardDescriptionText.text = so.tooltip;

        Debug.Log(so == null);
        cardSO = so;
    }

    public void GoldInit(int goldValue, float cardReverseTime) // 카드인데 돈만주는 / 피만회복시키는 카드면 Gold Init / Heal Init
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = goldSprite;
        rewardNameText.text = "돈";
        rewardDescriptionText.text = $"돈 {goldValue}을 얻습니다.";
        getGoldAmount = goldValue;
    }

    public void HealInit(float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = healSprite;
        rewardNameText.text = "회복";
        rewardDescriptionText.text = "체력을 전부 회복합니다.";
        getHeal = true;
    }

    public void PressButton() // called by Button or RewardManager OK Button
    {
        if (isGet) return;

        isGet = true;
        SoundManager.Instance.PlaySFX(SFXType.CardReverse, 0.3f);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(0, reverseTime / 2)); // 카드 1/2 뒤집기
        seq.AppendCallback(() => cover.gameObject.SetActive(false)); // 뒷부분 끄기
        seq.Append(transform.DOScaleX(1, reverseTime / 2)); // 카드 나머지 뒤집기
        seq.AppendCallback(() => isCompleteReverse = true); // 다뒤집었나요 변수 true
    }

    public void GetReward()
    {
        if (getGoldAmount > 0)
        {
            // Bezier로 돈UI로 날리기 도착하면 돈증가
            EffectManager.Instance.GetBezierCardEffect(transform.position, goldSprite, TargetType.GoldUI, () =>
            {
                goldValue.RuntimeValue += getGoldAmount;
                goldValueChanged.Occurred();
                ResetReward();
            });
        }
        else if (getHeal == true)
        {
            // Bezier로 회복으로 날리기 도착하면 회복
            EffectManager.Instance.GetBezierCardEffect(transform.position, healSprite, TargetType.HPUI, () =>
            {
                hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
                playerValueChanged.Occurred();
                ResetReward();
            });
        }
        else
        {
            EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, TargetType.Bag, () => { });
            GameManager.Instance.handleController.AddSpecial(cardSO.id);
            ResetReward();
        }
    }

    public void ResetReward() // 카드 안뽑을때 or 다뽑았을떄 해주기
    {
        gameObject.SetActive(false);
        transform.rotation = Quaternion.Euler(Vector3.zero); // 카드 돌리는거 초기화
        this.cardSO = null;
        rewardImage.sprite = null;
        rewardNameText.text = null;
        rewardDescriptionText.text = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isCompleteReverse)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isCompleteReverse)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
