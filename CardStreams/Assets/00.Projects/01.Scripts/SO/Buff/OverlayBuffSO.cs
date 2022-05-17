using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverlayBuffSO", menuName = "ScriptableObject/Buff/OverlayBuffSO")]
public class OverlayBuffSO : BuffSO
{
    public IntValue shieldValue;



    public override void UseBuff(int fieldValue)
    {
        shieldValue.RuntimeValue = fieldValue; // 값 들어올때 기존방패 + 얻은방패 합산해서들어옴

        // 칼 공격력만큼 회복
        // hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + swordValue.RuntimeValue, 0, hpValue.RuntimeMaxValue);
    }
}