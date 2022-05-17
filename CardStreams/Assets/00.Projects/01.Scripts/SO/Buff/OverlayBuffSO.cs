using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverlayBuffSO", menuName = "ScriptableObject/Buff/OverlayBuffSO")]
public class OverlayBuffSO : BuffSO
{
    public IntValue shieldValue;



    public override void UseBuff(int fieldValue)
    {
        shieldValue.RuntimeValue = fieldValue; // �� ���ö� �������� + �������� �ջ��ؼ�����

        // Į ���ݷ¸�ŭ ȸ��
        // hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + swordValue.RuntimeValue, 0, hpValue.RuntimeMaxValue);
    }
}