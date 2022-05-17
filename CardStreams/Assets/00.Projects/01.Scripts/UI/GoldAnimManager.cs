using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldAnimManager : MonoBehaviour
{
    public static GoldAnimManager Instance;

    private List<GoldAnimData> goldAnimDataList;

    public GameObject coinPrefab;
    private List<GameObject> coinObjList = new List<GameObject>();
    public GameObject coinObjParent;
    public GameObject goldTextObj;

    public IntValue goldValue;
    public EventSO FieldResetAfterEvnet;
    public EventSO GoldChangeEvent;



    [SerializeField] private float R = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void AddGoldAnim(GoldAnimData data)
    {
        goldAnimDataList.Add(data);
    }

    public IEnumerator JungSanCor(int amount, Vector3 pos)
    {
        // coin 생성
        for (int j = 0; j < amount; j++)
        {
            float angle = (360f / amount) * j;
            GameObject coinObj = coinObjList.Find(x => !x.activeSelf);

            if (coinObj == null)
            {
                coinObj = Instantiate(coinPrefab, coinObjParent.transform);
                coinObjList.Add(coinObj);
            }

            coinObj.SetActive(true);


            coinObj.transform.DOMove(pos, 0);

            pos.x = Mathf.Cos(angle * Mathf.Deg2Rad) * R + pos.x;
            pos.y = Mathf.Sin(angle * Mathf.Deg2Rad) * R + pos.y;
            coinObj.transform.DOMove(pos, 0.25f);
        }

        // 돈 들어오는 연출
        foreach (GameObject coinObj in coinObjList)
        {
            if (coinObj.activeSelf == true)
            {
                StartCoroutine(CoinMoveCor(coinObj));
            }
        }

        yield return new WaitForSeconds(1f);

        FieldResetAfterEvnet.Occurred();
    }

    private IEnumerator CoinMoveCor(GameObject coinObj)
    {
        coinObj.transform.DOMove(goldTextObj.transform.position, 0.5f);

        yield return new WaitForSeconds(0.5f);

        AddScore(1);

        coinObj.SetActive(false);
    }

    private void AddScore(int amount)
    {
        goldValue.RuntimeValue += amount;
        GoldChangeEvent.Occurred();
    }
}

public class GoldAnimData
{
    public int amount;
    public Vector3 pos;

    public GoldAnimData(int amount, Vector3 pos)
    {
        this.amount = amount;
        this.pos = pos;
    }
}
