using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    public GameObject spawnMobEffect;

    public GameObject jungSanEffect;

    public GameObject bezierCardEffect;
    [SerializeField] Transform specialCardTargetTrm; // 특수카드가 날아갈 목적지
    [SerializeField] Transform goldCardTargetTrm;   // 골드카드가 날아갈 목적지
    [SerializeField] Transform healCardTargetTrm;   // 회복카드가 날아갈 목적지
    [SerializeField] Transform expTargetTrm;        // 경험치가 날아갈 목적지

    public GameObject nextBuildEffect;
    private List<GameObject> nextBuildEffectList = new List<GameObject>();

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
        _mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
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

    /// <summary> 카드 생성해서 돌면서 작아지게하는마법 </summary>
    /// <param name="startPos">어디서 날아가나요</param>
    /// <param name="icon">카드의 아이콘</param>
    /// <param name="callback">카드 날라가는거 완료된후 터질 함수</param>
    /// <param name="rewardType">카드 날라가는거 완료된후 얻을 보상의 종류</param>
    public void GetBezierCardEffect(Vector3 startPos, Sprite icon, TargetType rewardType, Action callback, float speed = 1.3f, float radiusA = 6f, float radiusB = 10f)
    {
        startPos.z = 0; // canvas UI라서 z 문제생길수있음 그래서 0
        BezierCard effect = Instantiate(bezierCardEffect, startPos, Quaternion.identity, _mainCanvas.transform).GetComponent<BezierCard>();
        

        Transform targetTrm = null;

        switch (rewardType)
        {
            case TargetType.Handle:
                targetTrm = specialCardTargetTrm;
                break;

            case TargetType.GoldUI:
                targetTrm = goldCardTargetTrm;
                break;

            case TargetType.HPUI:
                targetTrm = healCardTargetTrm;
                break;

            case TargetType.Exp:
                targetTrm = expTargetTrm;
                break;

            default:
                Debug.Log("EffectManager - GetBezierCardEffect 에러");
                break;
        }

        effect.Init(targetTrm, icon, callback);

        Destroy(effect.gameObject, 15f); // 나중에는 PoolManager로 바꿔야해요
    }

    /// <summary> 유저가 지정한 특정 위치로 날아가기 </summary>
    /// <param name="targetTrm">특정 위치</param>
    public void GetBezierCardEffect(Vector3 startPos, Sprite icon, Transform targetTrm, Action callback)
    {
        startPos.z = 0; // canvas UI라서 z 문제생길수있음 그래서 0
        BezierCard effect = Instantiate(bezierCardEffect, startPos, Quaternion.identity, _mainCanvas.transform).GetComponent<BezierCard>();

        effect.Init(targetTrm, icon, callback);
        Destroy(effect.gameObject, 15f); // 나중에는 PoolManager로 바꿔야해요
    }

    public void GetNextBuildEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(nextBuildEffect, pos, Quaternion.identity);
        nextBuildEffectList.Add(effect);
    }
    public void DeleteNextBuildEffect()
    {
        for (int i = nextBuildEffectList.Count - 1; i >= 0; i--)
        {
            GameObject temp = nextBuildEffectList[i];
            nextBuildEffectList.RemoveAt(i);
            Destroy(temp);
        }

        nextBuildEffectList.Clear();
    }
}
