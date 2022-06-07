using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StreamEditor : EditorWindow
{
    IntValue goldValue;
    EventSO GoldChangeEvnet;
    
    [MenuItem("Window/StreamEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(StreamEditor));
    }
    private void OnGUI()
    {
        goldValue = Resources.Load<IntValue>("Gold");
        GoldChangeEvnet = Resources.Load<EventSO>("GoldChange");


        if(GUILayout.Button("Get Gold 100"))
        {
            goldValue.RuntimeValue += 100;
            GoldChangeEvnet.Occurred();
        }
    }
}
