using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    public GameObject spawnMobEffect;

    public GameObject jungSanEffect;

    public GameObject bezierCardEffect;

    private Canvas _mainCanvas; // 태그로 찾아요 나중엔싱글톤으로 찾아야할듯?

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

    private void Start()
    {
        _mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
    }

    public void GetSpawnMobEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(spawnMobEffect, pos, Quaternion.identity);
        Destroy(effect, 15f);
    }

    public void GetJungSanEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(jungSanEffect, pos, Quaternion.identity);
        Destroy(effect, 15f);
    }

    public void GetBezierCardEffect(Vector3 pos, Vector3 targetPos, Sprite icon, string text)
    {
        GameObject effect = Instantiate(bezierCardEffect, pos, Quaternion.identity, _mainCanvas.transform);

        Destroy(effect, 15f); // 나중에는 PoolManager로 바꿔야해요
    }
}
