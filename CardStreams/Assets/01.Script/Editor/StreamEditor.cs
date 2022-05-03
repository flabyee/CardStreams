using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StreamEditor : EditorWindow
{
    IntValue goldValue;
    EventSO TurnStartEvent;
    EventSO TurnEndEvent;
    EventSO MoveStartEvent;
    EventSO MoveEndEvent;
    EventSO NextTurnEvent;
    EventSO GoldChangeEvnet;
    
    [MenuItem("Window/StreamEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(StreamEditor));
    }
    private void OnGUI()
    {
        goldValue = Resources.Load<IntValue>("Gold");
        TurnStartEvent = Resources.Load<EventSO>("TurnStart");
        TurnEndEvent = Resources.Load<EventSO>("TurnEnd");
        MoveStartEvent = Resources.Load<EventSO>("MoveStart");
        MoveEndEvent = Resources.Load<EventSO>("MoveEnd");
        NextTurnEvent = Resources.Load<EventSO>("NextTurn");
        GoldChangeEvnet = Resources.Load<EventSO>("GoldChange");


        if (GUILayout.Button("TurnStart"))
        {
            TurnStartEvent.Occurred();
        }
        if (GUILayout.Button("TurnEnd"))
        {
            TurnEndEvent.Occurred();
        }
        if (GUILayout.Button("MoveStart"))
        {
            MoveStartEvent.Occurred();
        }
        if (GUILayout.Button("MoveEnd"))
        {
            MoveEndEvent.Occurred();
        }
        if (GUILayout.Button("NextTurn"))
        {
            NextTurnEvent.Occurred();
        }

        if(GUILayout.Button("Get Gold 100"))
        {
            goldValue.RuntimeValue += 100;
            GoldChangeEvnet.Occurred();
        }
    }
}
