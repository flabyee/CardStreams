using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI rewardNameText;
    [SerializeField] TextMeshProUGUI rewardDescriptionText;

    private RewardSO rewardSO;
    [SerializeField] IntValue goldValue;
    [SerializeField] IntValue hpValue;
    [SerializeField] EventSO playerValueChanged;
    [SerializeField] EventSO goldValueChanged;

    [HideInInspector] public SelectCardPanel selectPanel;

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
            saveData.speicialCardDataList[cardSO.id].haveAmount++;
        }

        SaveSystem.Save(saveData);

        selectPanel.Hide(); // ��ư�� ���� ������ �޾����� �θ� �г��� ����
    }

    public void SetReward(RewardSO so)
    {
        this.rewardSO = so;

        rewardImage.sprite = so.rewardSprite;
        rewardNameText.text = so.rewardName;
        rewardDescriptionText.text = so.rewardDescription;
    }

    public void ResetReward()
    {
        this.rewardSO = null;

        rewardImage.sprite = null;
        rewardNameText.text = null;
        rewardDescriptionText.text = null;
    }
}
