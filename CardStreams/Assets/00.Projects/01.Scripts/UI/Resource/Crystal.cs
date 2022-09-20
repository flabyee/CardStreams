using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI crystalText;

    private void Start()
    {
        UpdateCrystalText();
    }

    public void UpdateCrystalText() // ResourceManager Action에서 실행
    {
        crystalText.text = ResourceManager.Instance.crystal.ToString();
    }
}
