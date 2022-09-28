using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUIManager : MonoBehaviour
{
    public static VillageUIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Transform boxTrm; // ���������� �ڽ��� ��ġ
}
