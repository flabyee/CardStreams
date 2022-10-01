using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialHealPassive", menuName = "ScriptableObject/Passive/SpecialHeal")]

public class SpecialHealPassiveSO : PassiveSO
{
    [Header("��������")]
    public IntValue hpValue;
    public EventSO playerValueChanged;

    [Header("��ġ")]
    public int healAmount;

    public override void Init(Passive passive)
    {
        base.Init(passive);
        MissionObserverManager.instance.OnSpecialCard += OnSpecialCard;
        MissionObserverManager.instance.OffSpecialCard += OffSpecialCard;
    }

    private void OnSpecialCard(int specialCardId)
    {
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + healAmount * currentLevel, 0, hpValue.RuntimeMaxValue);
        playerValueChanged?.Occurred();
    }

    private void OffSpecialCard(int specialCardId)
    {
        hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue - healAmount * currentLevel, 0, hpValue.RuntimeMaxValue);
        playerValueChanged?.Occurred();
    }
}
