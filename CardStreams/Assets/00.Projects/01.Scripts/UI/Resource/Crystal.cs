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

    public void UpdateCrystalText() // ResourceManager Action���� ����
    {
        crystalText.text = ResourceManager.Instance.crystal.ToString();
    }
}
