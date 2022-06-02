using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    public GameObject spawnMobEffect;

    public GameObject jungSanEffect;

    [SerializeField] Transform handleTrm;
    [SerializeField] GameObject getCardPrefab;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GetSpawnMobEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(spawnMobEffect, pos, Quaternion.identity);
    }

    public void GetJungSanEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(jungSanEffect, pos, Quaternion.identity);
    }
}
