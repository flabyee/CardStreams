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
        string curText = "현재 차례 : ";
        string nextText = "다음 차례 : ";

        // curState와 nextState를 같이 바꿀 수는 없을까요?
        switch (curState)
        {
            case GameState.TurnStart:
                curText += "루프 시작";
                break;
            case GameState.TurnEnd:
                curText += "정산";
                break;
            case GameState.Move:
                curText += "루프";
                break;
            case GameState.Modify:
                curText += "정비";
                break;
            case GameState.Equip:
                curText += "건물 배치";
                break;
            case GameState.GameStart:
                curText += "게임 시작";
                break;
            default:
                break;
        }

        switch (nextState)
        {
            case GameState.TurnStart:
                nextText += "루프 시작";
                break;
            case GameState.TurnEnd:
                nextText += "루프 끝";
                break;
            case GameState.Move:
                nextText += "이동";
                break;
            case GameState.Modify:
                nextText += "정비";
                break;
            case GameState.Equip:
                nextText += "건물 배치";
                break;
            case GameState.GameStart:
                nextText += "게임 시작";
                break;
            default:
                break;
        }

        currentStateText.text = curText;
        nextStateText.text = nextText;
    }
}
