using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUIManager : MonoBehaviour
{
    public static VillageUIManager Instance { get; private set; }
    public Transform boxTrm; // 마을씬에서 박스의 위치
    public VillageRewardPanel rewardPanel;

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
