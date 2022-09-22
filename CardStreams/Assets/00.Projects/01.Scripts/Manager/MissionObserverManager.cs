using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionObserverManager : MonoBehaviour
{
    public static MissionObserverManager instance;

    public Action<int> UseSpecialCard;
    public Action<int> UnUseSpecialCard;

    private void Awake()
    {
        instance = this;
    }
}
