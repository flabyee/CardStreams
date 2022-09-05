using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentStateText;
    [SerializeField] TextMeshProUGUI nextStateText;

    private void Start()
    {
        GameManager.Instance.ChangeStateEvent += UpdateState;
    }

    private void UpdateState(GameState curState, GameState nextState)
    {
        string curText = "���� ���� : ";
        string nextText = "���� ���� : ";

        // curState�� nextState�� ���� �ٲ� ���� �������?
        switch (curState)
        {
            case GameState.TurnStart:
                curText += "���� ����";
                break;
            case GameState.TurnEnd:
                curText += "����";
                break;
            case GameState.Move:
                curText += "����";
                break;
            case GameState.Modify:
                curText += "����";
                break;
            case GameState.Equip:
                curText += "�ǹ� ��ġ";
                break;
            case GameState.GameStart:
                curText += "���� ����";
                break;
            default:
                break;
        }

        switch (nextState)
        {
            case GameState.TurnStart:
                nextText += "���� ����";
                break;
            case GameState.TurnEnd:
                nextText += "���� ��";
                break;
            case GameState.Move:
                nextText += "�̵�";
                break;
            case GameState.Modify:
                nextText += "����";
                break;
            case GameState.Equip:
                nextText += "�ǹ� ��ġ";
                break;
            case GameState.GameStart:
                nextText += "���� ����";
                break;
            default:
                break;
        }

        currentStateText.text = curText;
        nextStateText.text = nextText;
    }
}
