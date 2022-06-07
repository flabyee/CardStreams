using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardCard : MonoBehaviour
{
    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI rewardNameText;

    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;
    [SerializeField] EventSO playerValueChanged;
    [SerializeField] EventSO goldValueChanged;

    [HideInInspector] public SelectRewardManager selectPanel;
    private RewardSO rewardSO;
    private GameObject _createGetCardPrefab;
    private GameObject destination;

    

    public void SelectReward() // called by Button onclick, 버튼누를때 작동함
    {
        // 골드 증가
        if (rewardSO.goldReward > 0)
        {
            goldValue.RuntimeValue += rewardSO.goldReward;
            goldValueChanged.Occurred();
        }

        // 체력 모두 회복
        if (rewardSO.allHealReward == true)
        {
            hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
            playerValueChanged.Occurred();
        }

        // 카드 보상이 없다면 리턴
        if (rewardSO.cardReward.Length <= 0) return;

        // 세이브로드 + 카드 소매넣기
        SaveData saveData = SaveSystem.Load();

        foreach (var cardSO in rewardSO.cardReward)
        {
            RectTransform getCard = Instantiate(_createGetCardPrefab, Vector2.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("MainCanvas").transform).GetComponent<RectTransform>();

            getCard.transform.position = new Vector3(getCard.transform.position.x, getCard.transform.position.y, 0);

            BezierCurve bezier = getCard.GetComponent<BezierCurve>();
            GetRewardCard getRewardCard = getCard.GetComponent<GetRewardCard>();

            getRewardCard.Init(rewardImage.sprite, rewardNameText.text);
            bezier.StartBezier(destination); // 베지어 돌려요
            getCard.DORotate(new Vector3(0, 0, 180), 1f);
            getCard.DOScale(0.3f, 1f).OnComplete(() => Destroy(getCard.gameObject));

            saveData.speicialCardDataList[cardSO.id].haveAmount++;
        }

        SaveSystem.Save(saveData);

        selectPanel.Hide(); // 버튼을 눌러 보상을 받았으니 부모 패널을 꺼요
    }

    public void SetReward(RewardSO so, GameObject cardPrefab, GameObject getTarget)
    {
        this.rewardSO = so;

        _createGetCardPrefab = cardPrefab;

        destination = getTarget;

        rewardImage.sprite = so.rewardSprite;
        rewardNameText.text = so.rewardName;
    }

    public void ResetReward()
    {
        this.rewardSO = null;

        rewardImage.sprite = null;
        rewardNameText.text = null;
    }
}
