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
    /// <param name="startPos">��� ���ư�����</param>
    /// <param name="icon">ī���� ������</param>
    /// <param name="cardID">ī���� ID, ���󰡼� ������ �����ϸ� ī��ȹ��. -1�̸� �ƹ��͵� ȹ�� ����</param>
    public void GetBezierCardEffect(Vector3 startPos, Sprite icon, int cardID, CardType cardType)
    {
        startPos.z = 0; // canvas UI�� z ������������� �׷��� 0
        BezierCard effect = Instantiate(bezierCardEffect, startPos, Quaternion.identity, _mainCanvas.transform).GetComponent<BezierCard>();

        effect.Init(targetTrm, icon, cardID, cardType);

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
