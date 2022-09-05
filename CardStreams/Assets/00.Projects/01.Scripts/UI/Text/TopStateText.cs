using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopStateText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stateText;

    private void Start()
    {
        GameManager.Instance.ChangeStateEvent += UpdateState;
    }

    private void UpdateState(GameState curState, GameState nextState)
    {
        string stateText = "���� ���� : ";
        switch (curState)
        {
            case GameState.TurnStart:
                stateText += "�غ�";
                break;
            case GameState.TurnEnd:
                stateText += "����";
                break;
            case GameState.Move:
                stateText += "����";
                break;
            case GameState.Modify:
                stateText += "����";
                break;
            case GameState.Equip:
                stateText += "��ġ";
                break;
            case GameState.GameStart:
                stateText += "����";
                break;
            default:
                break;
        }
        this.stateText.text = stateText;
    }
}
