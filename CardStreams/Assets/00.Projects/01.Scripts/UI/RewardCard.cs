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

    

    public void SelectReward() // called by Button onclick, ��ư������ �۵���
    {
        // ��� ����
        if (rewardSO.goldReward > 0)
        {
            goldValue.RuntimeValue += rewardSO.goldReward;
            goldValueChanged.Occurred();
        }

        // ü�� ��� ȸ��
        if (rewardSO.allHealReward == true)
        {
            hpValue.RuntimeValue = hpValue.RuntimeMaxValue;
            playerValueChanged.Occurred();
        }

        // ī�� ������ ���ٸ� ����
        if (rewardSO.cardReward.Length <= 0) return;

        // ���̺�ε� + ī�� �Ҹųֱ�
        SaveData saveData = SaveSystem.Load();

        foreach (var cardSO in rewardSO.cardReward)
        {
            RectTransform getCard = Instantiate(_createGetCardPrefab, Vector2.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("MainCanvas").transform).GetComponent<RectTransform>();

            getCard.transform.position = new Vector3(getCard.transform.position.x, getCard.transform.position.y, 0);

            BezierCurve bezier = getCard.GetComponent<BezierCurve>();
            GetRewardCard getRewardCard = getCard.GetComponent<GetRewardCard>();

            getRewardCard.Init(rewardImage.sprite, rewardNameText.text);
            bezier.StartBezier(destination); // ������ ������
            getCard.DORotate(new Vector3(0, 0, 180), 1f);
            getCard.DOScale(0.3f, 1f).OnComplete(() => Destroy(getCard.gameObject));

            saveData.speicialCardDataList[cardSO.id].haveAmount++;
        }

        SaveSystem.Save(saveData);

        selectPanel.Hide(); // ��ư�� ���� ������ �޾����� �θ� �г��� ����
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
