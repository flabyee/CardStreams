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
        string stateText = "현재 차례 : ";
        switch (curState)
        {
            case GameState.TurnStart:
                stateText += "준비";
                break;
            case GameState.TurnEnd:
                stateText += "종료";
                break;
            case GameState.Move:
                stateText += "전투";
                break;
            case GameState.Modify:
                stateText += "상점";
                break;
            case GameState.Equip:
                stateText += "배치";
                break;
            case GameState.GameStart:
                stateText += "시작";
                break;
            default:
                break;
        }
        this.stateText.text = stateText;
    }
}
