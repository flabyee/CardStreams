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
    [SerializeField] Transform specialCardTargetTrm; // Ư��ī�尡 ���ư� ������
    [SerializeField] Transform goldCardTargetTrm;   // ���ī�尡 ���ư� ������
    [SerializeField] Transform healCardTargetTrm;   // ȸ��ī�尡 ���ư� ������
    [SerializeField] Transform expTargetTrm;        // ����ġ�� ���ư� ������

    public GameObject nextBuildEffect;
    private List<GameObject> nextBuildEffectList = new List<GameObject>();

    private Canvas _mainCanvas; // �±׷� ã�ƿ� ���߿��̱������� ã�ƾ��ҵ�?

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

    /// <summary> ī�� �����ؼ� ���鼭 �۾������ϴ¸��� </summary>
    /// <param name="startPos">��� ���ư�����</param>
    /// <param name="icon">ī���� ������</param>
    /// <param name="callback">ī�� ���󰡴°� �Ϸ���� ���� �Լ�</param>
    /// <param name="rewardType">ī�� ���󰡴°� �Ϸ���� ���� ������ ����</param>
    public void GetBezierCardEffect(Vector3 startPos, Sprite icon, TargetType rewardType, Action callback, float speed = 1.3f, float radiusA = 6f, float radiusB = 10f)
    {
        startPos.z = 0; // canvas UI�� z ������������� �׷��� 0
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
                Debug.Log("EffectManager - GetBezierCardEffect ����");
                break;
        }

        effect.Init(targetTrm, icon, callback);

        Destroy(effect.gameObject, 15f); // ���߿��� PoolManager�� �ٲ���ؿ�
    }

    /// <summary> ������ ������ Ư�� ��ġ�� ���ư��� </summary>
    /// <param name="targetTrm">Ư�� ��ġ</param>
    public void GetBezierCardEffect(Vector3 startPos, Sprite icon, Transform targetTrm, Action callback)
    {
        startPos.z = 0; // canvas UI�� z ������������� �׷��� 0
        BezierCard effect = Instantiate(bezierCardEffect, startPos, Quaternion.identity, _mainCanvas.transform).GetComponent<BezierCard>();

        effect.Init(targetTrm, icon, callback);
        Destroy(effect.gameObject, 15f); // ���߿��� PoolManager�� �ٲ���ؿ�
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
