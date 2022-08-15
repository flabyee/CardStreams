using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static IEnumerator DelayCoroutine(float delayTime, Action completeCallback)
    {
        yield return new WaitForSeconds(delayTime);
        completeCallback?.Invoke();
    }
}