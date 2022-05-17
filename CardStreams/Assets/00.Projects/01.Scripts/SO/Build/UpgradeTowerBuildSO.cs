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
            field.cardPower.SetValue(field.cardPower.value + addAmount);

            TextMeshProUGUI text = field.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            text.text = field.cardPower.value.ToString();
            text.color = Color.red;

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }

    public override void AccessPlayer(Player player)
    {

    }
}
