using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StatPanel : MonoBehaviour
{
    [Header("�гκ� �ٸ� ������")]
    [SerializeField] bool isRuntimeMaxValueExist = false;
    [SerializeField] IntValue statIntValue; // Ĩ�� ����(��������)

    [Header("�г� �Ʒ� �̹����� �ؽ�Ʈ")]
    [SerializeField] TextMeshProUGUI runtimeValueText;
    [SerializeField] TextMeshProUGUI runtimeMaxValueText;
    [SerializeField] Image[] runtimeStatImages; // �����Ҷ� �ߴ� Ĩ ����, �ϴ� 20���� ����
    [SerializeField] Sprite[] statUnitSprites; // 1 | 10 | 50 | 100 | 500 | 1000 ����, ������ 1�̶� 10�� ����

    public void StatUIChange() // Called by EventSO from gameObject | ���ӿ�����Ʈ�� ����ִ� EventSO���� ȣ���
    {
        int statAmount = statIntValue.RuntimeValue;

        int count = 0;

        runtimeValueText.text = statIntValue.RuntimeValue.ToString();
        if (isRuntimeMaxValueExist) runtimeMaxValueText.text = statIntValue.RuntimeMaxValue.ToString();

        int divide10 = statAmount / 10;
        for (int i = 0; i < divide10; i++)
        {
            runtimeStatImages[i].DOFade(1, 0.2f);
            runtimeStatImages[i].sprite = statUnitSprites[1]; // 10 �̹����� ��ü

            count++;
        } // count 1

        statAmount -= divide10 * 10; // 3

        int divide1 = statAmount; // 3
        for (int i = count; i < divide1 + divide10; i++) // 1 < 3
        {
            runtimeStatImages[i].DOFade(1, 0.2f);
            runtimeStatImages[i].sprite = statUnitSprites[0]; // 1 �̹����� ��ü

            count++;
        }

        for (int i = count; i < runtimeStatImages.Length; i++)
        {
            int avoidClosure = i;
            runtimeStatImages[avoidClosure].DOFade(0, 0.2f).OnComplete( () => runtimeStatImages[avoidClosure].sprite = null);

            count++;
        }
    }
}
