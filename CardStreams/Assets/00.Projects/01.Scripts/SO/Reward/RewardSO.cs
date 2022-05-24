using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardSO", menuName = "ScriptableObject/Reward/Reward")]
public class RewardSO : ScriptableObject
{
    public Sprite rewardSprite;
    public string rewardName;
    [TextArea] public string rewardDescription;
    public int moneyReward;
    public bool allHealReward;
    public SpecialCardSO[] cardReward;
}
