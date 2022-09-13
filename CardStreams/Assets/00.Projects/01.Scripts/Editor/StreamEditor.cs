using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StreamEditor : EditorWindow
{
    IntValue goldValue;
    EventSO GoldChangeEvnet;
    ShopController shopController;

    [MenuItem("Window/StreamEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(StreamEditor));
    }
    private void OnGUI()
    {
        goldValue = Resources.Load<IntValue>("Gold");
        GoldChangeEvnet = Resources.Load<EventSO>("GoldChange");
        shopController = FindObjectOfType<ShopController>();

        if (GUILayout.Button("Get Gold 100"))
        {
            goldValue.RuntimeValue += 100;
            GoldChangeEvnet.Occurred();
        }
        if (GUILayout.Button("Upgrade Shop"))
        {
            shopController.UpgradeShop();
        }

        if(GUILayout.Button("Get Gold 100 in menu"))
        {
            SaveData saveData = SaveSystem.Load();
            saveData.crystal += 100;
            SaveSystem.Save(saveData);
        }
        if(GUILayout.Button("Reset Remove Count"))
        {
            SaveData saveData = SaveSystem.Load();
            saveData.maxRemoveCount = 0;
            SaveSystem.Save(saveData);
        }
    }
}
