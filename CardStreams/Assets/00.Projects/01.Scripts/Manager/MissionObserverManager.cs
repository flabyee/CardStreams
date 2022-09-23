using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionObserverManager : MonoBehaviour
{
    public static MissionObserverManager instance;

    public Action<BasicCard> OnBasicCard;
    public Action<int> OnShield;

    public Action<int> OnSpecialCard;
    public Action<int> OffSpecialCard;

    public Action<int> TimerEvent;
    private int timer;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(TimerCor());
    }

    private void Update()
    {
        
    }

    public void ResetTimer()
    {
        timer = 0;
        TimerEvent?.Invoke(timer);
    }

    IEnumerator TimerCor()
    {
        WaitForSeconds timerSecond = new WaitForSeconds(1f);

        while(true)
        {
            yield return timerSecond;

            timer++;
            TimerEvent?.Invoke(timer);
        }
    }
}
