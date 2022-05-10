using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestSceneLoad : MonoBehaviour
{
    public IntValue stageNumValue;
    public InputField stageNumInputField;

    public void GameStart()
    {
        stageNumValue.RuntimeValue = int.Parse(stageNumInputField.text) - 1;

        SceneManager.LoadScene(1);
    }
}
