using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    public GameObject spawnMobEffect;

    public GameObject jungSanEffect;

    public GameObject bezierCardEffect;
    [SerializeField] Transform targetTrm;

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

    /// <summary> ī�� �����ؼ� ���鼭 �۾������ϴ¸��� </summary>
    /// <param name="targetTrm">���� ���ư���</param>
    /// <param name="icon">ī���� ������</param>
    /// <param name="text">ī���� �̸�</param>
    public void GetBezierCardEffect(Sprite icon, string text)
    {
        BezierCard effect = Instantiate(bezierCardEffect, Vector3.zero, Quaternion.identity, _mainCanvas.transform).GetComponent<BezierCard>();

        effect.Init(targetTrm, icon, text);

        Destroy(effect.gameObject, 15f); // ���߿��� PoolManager�� �ٲ���ؿ�
    }
}
