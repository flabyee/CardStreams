using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "UpgradeTowerBuild", menuName = "ScriptableObject/Build/UpgradeTowerBuild")]
public class UpgradeTowerBuildSO : BuildSO
{
    [Header("Amount")]
    public int addAmount;

    public override void AccessCard(Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        cardPower.SetValue(cardPower.value + addAmount);

            TextMeshProUGUI text = field.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            text.text = cardPower.value.ToString();
            text.color = Color.red;

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }

    public override void AccessPlayer(Player player)
    {

    }
}
