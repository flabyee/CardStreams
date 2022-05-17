using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldAnimManager : MonoBehaviour
{
    public static GoldAnimManager Instance;

    public GameObject coinPrefab;
    private List<GameObject> coinObjList = new List<GameObject>();
    public GameObject coinObjParent;
    public GameObject goldTextObj;

    [Header("System")]
    public float coinCreateDelay;
    [SerializeField] private int earnSystemCount;
    private int count;  // ����Ʈ�� �߰��Ҷ����� �̰� ���ϰ� �̰� 2�� ��� ����

    public IntValue goldValue;
    public EventSO FieldResetAfterEvnet;
    public EventSO GoldChangeEvent;



    [SerializeField] private float R = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateCoin(int amount, Vector3 centerPos)
    {
        // coin ����
        for (int j = 0; j < amount; j++)
        {
            float theta = (360f / amount) * j;
            GameObject coinObj = coinObjList.Find(x => !x.activeSelf);

            if (coinObj == null)
            {
                coinObj = Instantiate(coinPrefab, coinObjParent.transform);
                coinObjList.Add(coinObj);
            }

            coinObj.transform.position = centerPos;

            coinObj.SetActive(true);

            Vector3 movePos = Vector3.zero;
            movePos.x = Mathf.Cos(theta * Mathf.Deg2Rad) * R + centerPos.x;
            movePos.y = Mathf.Sin(theta * Mathf.Deg2Rad) * R + centerPos.y;
            coinObj.transform.DOMove(movePos, 0.25f);
        }
    }

    public void GetAllCoin()
    {
        count++;
        if(count == earnSystemCount)
        {
            count = 0;

            StartCoroutine(GetCoinCor());
        }
    }

    private IEnumerator GetCoinCor()
    {
        // �� ������ ����
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