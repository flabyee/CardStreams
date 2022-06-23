using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum FieldState
{
    NULL,
    yet,    // 아직 설치안한 -> able
    able,   // 현재 설치가능한 -> 설치하면 not
    not,    // 설치 불가능한, 시작하자마자 설정 될때도 있다
    randomMob,  // 랜덤하게 몹을 생성하겠다 <- 지금은 안쓰는듯?
}

// 1. 모두 yet 2. not 생성, 3. 차례차례 able 
//    able 인곳에만 설치가능

public class Field : MonoBehaviour
{
    public Image background;
    public Image colorImage;

    public FieldState fieldState = FieldState.NULL;

    public bool isSet;
    public DragbleCard dragbleCard;
    public CardPower cardPower;

    public Action<Player> accessBuildToPlayerAfterOnField;
    public Action<Player, Field> accessBeforeOnField;
    public Action<Field> accessBuildToCardAfterMoveStart;

    public int tileNum;

    [Header("Debug")]
    public DropArea dropArea;

    // 기본적으론 이걸로 정보 넣고
    public void Init(CardPower power, DragbleCard dragCard)
    {
        isSet = true;
        cardPower = power;
        dragbleCard = dragCard;

        colorImage.color = ConstManager.Instance.basicTypeColorList[(int)(power as BasicCard).basicType];
    }

    // 랜덤몹생성같이 필드의 상태도 변해야 하면 이걸 쓴다
    public void Init(CardPower power, DragbleCard dragCard, FieldState type)
    {
        isSet = true;
        cardPower = power;
        dragbleCard = dragCard;
        fieldState = type;

        colorImage.color = ConstManager.Instance.basicTypeColorList[(int)(power as BasicCard).basicType];

        if(type == FieldState.randomMob)
        {
            colorImage.color = Color.magenta;
        }
    }

    public void ResetData()
    {
        isSet = false;
        cardPower = null;
        dragbleCard = null;

        colorImage.color = Color.white;
    }
    public void OnBuildAccess()
    {
        accessBuildToCardAfterMoveStart?.Invoke(this);
    }

    public void FieldReset()
    {
        if(isSet == true)
        {
            dragbleCard.ActiveFalse();
        }

        ResetData();
    }

}
