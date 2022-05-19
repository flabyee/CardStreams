using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatPanel : MonoBehaviour
{
    [SerializeField] Image[] runtimeStatImages; // 게임할때 뜨는 칩 개수, 일단 20개만 넣음

    [SerializeField] Sprite[] statUnitSprites; // 1 | 10 | 50 | 100 | 500 | 1000 단위, 지금은 1이랑 10만 넣음

    [SerializeField] IntValue statIntValue; // 칩의 종류(스텟종류)


    public void StatUIChange() // Called by EventSO from gameObject | 게임오브젝트에 들어있는 EventSO에서 호출됨
    {
        int statAmount = statIntValue.RuntimeValue;

        int count = 0;

        int divide10 = statAmount / 10;
        for (int i = 0; i < divide10; i++)
        {
            runtimeStatImages[i].sprite = statUnitSprites[1]; // 10 이미지로 교체

            count++;
        } // count 1

        statAmount -= divide10 * 10; // 3

        int divide1 = statAmount; // 3
        for (int i = count; i < divide1 + divide10; i++) // 1 < 3
        {
            runtimeStatImages[i].sprite = statUnitSprites[0]; // 1 이미지로 교체

            count++;
        }

        for (int i = count; i < runtimeStatImages.Length; i++)
        {

            runtimeStatImages[i].sprite = null;

            count++;
        }
    }
}
