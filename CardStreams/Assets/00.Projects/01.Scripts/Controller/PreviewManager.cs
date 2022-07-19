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

        // ���� ��, ���� ����
        int originHP = hpValue.RuntimeValue;
        int originSword = swordValue.RuntimeValue;
        int originShield = shieldValue.RuntimeValue;

        // temp�� ���� ����

        // move �� ��� ����
        Queue<Values> valueList = Move(moveIndex);

        // ui�� ǥ��
        foreach(Values values in valueList)
        {
            Debug.Log($"hp : {values.hp}, swrod : {values.sword}, shield : {values.shield}");
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
                if (buildCard.isAcceseCard == true)
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
