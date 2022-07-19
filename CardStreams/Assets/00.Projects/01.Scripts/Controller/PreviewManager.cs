using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Values
{
    public int hp;
    public int sword;
    public int shield;

    public Values(int hp, int sword, int shield)
    {
        this.hp = hp;
        this.sword = sword;
        this.shield = shield;
    }
}

public class PreviewManager : MonoBehaviour
{
    int maxMoveCount = 4;

    Player tempPlayer;

    IntValue hpValue;
    IntValue swordValue;
    IntValue shieldValue;

    private void Awake()
    {
        tempPlayer = GetComponent<Player>();

        hpValue = tempPlayer.hpValue;
        swordValue = tempPlayer.swordValue;
        shieldValue = tempPlayer.shieldValue;
    }

    public void OnPreviewMode(int moveIndex)
    {
        moveIndex = GameManager.Instance.moveIndex;

        // 기존 값, 버프 저장
        int originHP = hpValue.RuntimeValue;
        int originSword = swordValue.RuntimeValue;
        int originShield = shieldValue.RuntimeValue;

        // temp에 버프 적용

        // move 한 결과 저장
        Queue<Values> valueList = Move(moveIndex);

        // ui로 표시
        foreach(Values values in valueList)
        {
            Debug.Log($"hp : {values.hp}, swrod : {values.sword}, shield : {values.shield}");
        }

        // 값, 버프 되돌리기
        hpValue.RuntimeValue = originHP;
        swordValue.RuntimeValue = originSword;
        shieldValue.RuntimeValue = originShield;

        tempPlayer.playerValueChangeEvent.Occurred();
    }

    public void OffPreviewMode()
    {

    }

    private Queue<Values> Move(int moveIndex)
    {
        Queue<Values> valueList = new Queue<Values>();

        // 필드에 건물 효과 적용
        for (int i = 0; i < maxMoveCount; i++)
        {
            Field field = MapManager.Instance.fieldList[moveIndex + i];

            foreach (BuildCard buildCard in field.accessBuildList)
            {
                if (buildCard.isAcceseCard == true)
                {
                    buildCard.AcceseCard(field);
                }
            }
        }


        for (int i = 0; i < maxMoveCount; i++)
        {
            Field field = MapManager.Instance.fieldList[moveIndex];

            // 플레이어한테 필드 효과 적용 
            if (field.isSet == true)
            {
                tempPlayer.OnFeild(field);
            }

            // 플레이어한테 건물효과 적용
            foreach (BuildCard buildCard in field.accessBuildList)
            {
                if (buildCard.isAccesePlayer == true)
                {
                    buildCard.AccesePlayer(tempPlayer);
                }
            }

            valueList.Enqueue(new Values(hpValue.RuntimeValue,
                swordValue.RuntimeValue, shieldValue.RuntimeValue));

            moveIndex++;
        }

        return valueList;
    }
}
