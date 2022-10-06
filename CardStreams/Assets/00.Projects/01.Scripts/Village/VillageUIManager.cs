using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUIManager : MonoBehaviour
{
    public static VillageUIManager Instance { get; private set; }
    public Transform boxTrm; // ���������� �ڽ��� ��ġ
    public VillageRewardPanel rewardPanel;
    public BuildingInfoPanel buildInfoPanel;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
