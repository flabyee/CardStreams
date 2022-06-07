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

    public FieldState fieldState = FieldState.NULL;


    public DragbleCard dragbleCard;
    public CardPower cardPower;

    public Action<Player> accessBuildToPlayerAfterOnField;
    public Action<Player, Field> accessBeforeOnField;
    public Action<Field> accessBuildToCardAfterMoveStart;

    [Header("Debug")]
    public DropArea dropArea;

    public void Init(CardPower power, DragbleCard dragCard, FieldState type)
    {
        cardPower = power;
        dragbleCard = dragCard;
        fieldState = type;
    }

    public void ResetData()
    {
        cardPower = null;
        dragbleCard = null;
    }
    public void OnBuildAccess()
    {
        accessBuildToCardAfterMoveStart?.Invoke(this);
    }

    public void FieldReset()
    {
        if(dragbleCard != null)
        {
            Debug.Log("reset");
            dragbleCard.ActiveFalse();
        }

        ResetData();
    }

}
