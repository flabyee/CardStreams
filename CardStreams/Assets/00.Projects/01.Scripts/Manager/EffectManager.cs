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

    /// <summary> 카드 생성해서 돌면서 작아지게하는마법 </summary>
    /// <param name="startPos">어디서 날아가나요</param>
    /// <param name="icon">카드의 아이콘</param>
    /// <param name="cardID">카드의 ID, 날라가서 목적지 도착하면 카드획득. -1이면 아무것도 획득 안함</param>
    public void GetBezierCardEffect(Vector3 startPos, Sprite icon, int cardID, CardType cardType)
    {
        startPos.z = 0; // canvas UI라서 z 문제생길수있음 그래서 0
        BezierCard effect = Instantiate(bezierCardEffect, startPos, Quaternion.identity, _mainCanvas.transform).GetComponent<BezierCard>();

        effect.Init(targetTrm, icon, cardID, cardType);

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
