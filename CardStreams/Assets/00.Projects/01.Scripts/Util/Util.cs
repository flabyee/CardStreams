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

    public static void RandomList<T>(ref List<T> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}