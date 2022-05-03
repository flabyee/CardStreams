using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum FieldType
{
    NULL,
    yet,    // 아직 설치안한 -> able
    able,   // 현재 설치가능한 -> 설치하면 not
    not,    // 설치 불가능한, 시작하자마자 설정 될때도 있다
    randomMob,  // 랜덤하게 몹을 생성하겠다
}

// 1. 모두 yet 2. not 생성, 3. 차례차례 able 
//    able 인곳에만 설치가능

public class Field : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    public FieldType fieldType = FieldType.NULL;

    public CardType cardType = CardType.NULL;
    public int value;

    public CardPower cardPower;

    public Action<Player> accessBuildToPlayerAfterOnField;
    public Action<Player, Field> accessBeforeOnField;
    public Action<Field> accessBuildToCardAfterMoveStart;

    [HideInInspector] public DropArea dropArea;

    private void Start()
    {
    }

    public void SetData(CardPower cardPower)
    {
        cardType = cardPower.cardType;
        value = cardPower.value;

        //image.sprite = sprite;
        //text.text = value.ToString();
    }
    public void ResetData()
    {
        cardPower = null;
        cardType = CardType.NULL;
        value = 0;
    }
    public void OnAccessCard()
    {
        accessBuildToCardAfterMoveStart?.Invoke(this);
    }
    public void OnfieldReset()
    {
        if(transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        ResetData();
    }
}
