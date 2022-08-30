using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

public class PreviewController : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    int maxMoveCount = 4;

    BuffController playerBuffCon;
    public Player tempPlayer;
    public BuffController tempPlayerBuffCon;

    IntValue hpValue;
    IntValue swordValue;
    IntValue shieldValue;

    private void Start()
    {
        playerBuffCon = GameManager.Instance.player.GetComponent<BuffController>();

        hpValue = tempPlayer.hpValue;
        swordValue = tempPlayer.swordValue;
        shieldValue = tempPlayer.shieldValue;
    }

    public void OnPreviewMode()
    {
        resultText.text = string.Empty;

        int moveIndex = GameManager.Instance.moveIndex;

        // 기존 값, 버프 저장
        int originHP = hpValue.RuntimeValue;
        int originSword = swordValue.RuntimeValue;
        int originShield = shieldValue.RuntimeValue;

        Queue<Buff> originBuffList = new Queue<Buff>();
        foreach(Buff buff in playerBuffCon.GetBuffList())
        {
            originBuffList.Enqueue(buff.GetCopyBuff());
        }

        // temp에게 버프 적용
        tempPlayerBuffCon.RemoveAllBuff();
        foreach(Buff buff in originBuffList)
        {
            tempPlayerBuffCon.AddBuff(buff);
        }

        // move 한 결과 저장
        Queue<Values> valueList = Move(moveIndex);

        // ui로 표시
        foreach(Values values in valueList)
        {
            print($"hp : {values.hp}, swrod : {values.sword}, shield : {values.shield}");

            resultText.text += $"{values.hp}/{values.sword}/{values.shield}" + "\n";
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
                if (buildCard.isAccessCard == true)
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
                if (buildCard.isAccessPlayer == true)
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
