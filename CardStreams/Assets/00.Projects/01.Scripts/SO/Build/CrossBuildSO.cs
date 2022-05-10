using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "CrossBuild", menuName = "ScriptableObject/Build/CrossBuild")]
public class CrossBuildSO : BuildSO
{
    public override void AccessCard(Field field)
    {
        if (field.cardPower.cardType == CardType.Monster)
        {
            field.cardPower.value -= 1;

            TextMeshProUGUI text = field.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            text.text = field.cardPower.value.ToString();
            text.color = Color.red;

            OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);

        }
    }

    public override void AccessPlayer(Player player)
    {
    }
}