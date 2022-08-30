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

        // ���� ��, ���� ����
        int originHP = hpValue.RuntimeValue;
        int originSword = swordValue.RuntimeValue;
        int originShield = shieldValue.RuntimeValue;

        Queue<Buff> originBuffList = new Queue<Buff>();
        foreach(Buff buff in playerBuffCon.GetBuffList())
        {
            originBuffList.Enqueue(buff.GetCopyBuff());
        }

        // temp���� ���� ����
        tempPlayerBuffCon.RemoveAllBuff();
        foreach(Buff buff in originBuffList)
        {
            tempPlayerBuffCon.AddBuff(buff);
        }

        // move �� ��� ����
        Queue<Values> valueList = Move(moveIndex);

        // ui�� ǥ��
        foreach(Values values in valueList)
        {
            print($"hp : {values.hp}, swrod : {values.sword}, shield : {values.shield}");

            resultText.text += $"{values.hp}/{values.sword}/{values.shield}" + "\n";
        }

        // ��, ���� �ǵ�����
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

        // �ʵ忡 �ǹ� ȿ�� ����
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

            // �÷��̾����� �ʵ� ȿ�� ���� 
            if (field.isSet == true)
            {
                tempPlayer.OnFeild(field);
            }

            // �÷��̾����� �ǹ�ȿ�� ����
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
