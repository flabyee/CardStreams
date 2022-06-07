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
    /// <param name="targetTrm">어디로 날아가나</param>
    /// <param name="icon">카드의 아이콘</param>
    /// <param name="text">카드의 이름</param>
    public void GetBezierCardEffect(Sprite icon, string text)
    {
        BezierCard effect = Instantiate(bezierCardEffect, Vector3.zero, Quaternion.identity, _mainCanvas.transform).GetComponent<BezierCard>();

        effect.Init(targetTrm, icon, text);

        Destroy(effect.gameObject, 15f); // 나중에는 PoolManager로 바꿔야해요
    }
}
