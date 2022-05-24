using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardPanel : MonoBehaviour
{
    [SerializeField] RewardCard[] rewardCards;
    private CanvasGroup _cg;
    

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();

        Hide();
    }

    public void Show()
    {
        _cg.alpha = 1;
        _cg.blocksRaycasts = true;
        _cg.interactable = true;
    }

    public void Hide()
    {
        _cg.alpha = 0;
        _cg.blocksRaycasts = false;
        _cg.interactable = false;
    }

    public void GetSpecialCard(SpecialCardSO so)
    {
        SaveData saveData = SaveSystem.Load();
        saveData.speicialCardDataList[so.id].haveAmount++;
        SaveSystem.Save(saveData);
    }

    public void InitReward()
    {

    }
}
