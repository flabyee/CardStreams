using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum TargetType // 보상카드가 날아갈 위치
{
    Handle,
    GoldUI,
    HPUI
}

public class RewardCard : MonoBehaviour
{
    [SerializeField] GameObject cover; // 카드 누르기전 앞면

    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI rewardNameText;


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

    
    private float reverseTime = 1f;
    public bool isget = false;


    private SpecialCardSO cardSO;
    
    private void DefaultInit(float cardReverseTime) // 카드 뽑을때 해주기(시작시에잠깐)
    {
        cover.SetActive(true);
        gameObject.SetActive(true);
        reverseTime = cardReverseTime;
        isget = false;
    }

    public void CardInit(SpecialCardSO so, float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = so.sprite;
        rewardNameText.text = so.specialCardName;

        Debug.Log(so == null);
        cardSO = so;
    }

    public void GoldInit(int goldValue, float cardReverseTime) // 카드인데 돈만주는 / 피만회복시키는 카드면 Gold Init / Heal Init
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = goldSprite;
        rewardNameText.text = "돈";
        getGoldAmount = goldValue;
    }

    public void HealInit(float cardReverseTime)
    {
        DefaultInit(cardReverseTime);

        rewardImage.sprite = healSprite;
        rewardNameText.text = "회복";
        getHeal = true;
    }

    public void PressButton() // called by Button
    {
        if (isget) return;

        isget = true;
        SoundManager.Instance.PlaySFX(SFXType.CardReverse);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(0, reverseTime / 2)); // 카드 1/2 뒤집기
        seq.AppendCallback(() => cover.gameObject.SetActive(false)); // 뒷부분 끄기
        seq.Append(transform.DOScaleX(1, reverseTime / 2)); // 카드 나머지 뒤집기
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
            Debug.Log(cardSO);
            Debug.Log(cardSO.sprite);
            Debug.Log(cardSO.id);
            EffectManager.Instance.GetBezierCardEffect(transform.position, cardSO.sprite, TargetType.Handle, () => { });
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
    }
}
