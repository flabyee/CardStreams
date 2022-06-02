using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GoldAnimManager : MonoBehaviour
{
    public static GoldAnimManager Instance;

    private List<GameObject> coinObjList = new List<GameObject>();

    [Header("UI")]
    public GameObject coinPrefab;
    public GameObject coinObjParent;
    public GameObject goldTextObj;

    public Text originText;
    public Text earnText;
    public Text resultText;

    public GameObject resultPanel;

    [Header("System")]
    [SerializeField] private int earnSystemCount;
    private int count;  // 리스트에 추가할때마다 이걸 더하고 이게 2면 골드 생성

    private int originGold;

    public IntValue goldValue;
    public EventSO FieldResetAfterEvnet;
    public EventSO GoldChangeEvent;

    [SerializeField] Sprite[] coinSprites; // 1 10 100 코인 Sprite
    private int allCoinAmount;

    [SerializeField] private float R = 0.5f;

    private void Awake()
    {
        Instance = this;

        HideResult();
    }

    public void CreateCoin(int amount, Vector3 centerPos)
    {
        // coin 생성
        allCoinAmount += amount;

        int coinSum = amount;
        List<Sprite> coinCreateList = new List<Sprite>();

        for (int coinCount = coinSprites.Length - 1; coinCount >= 0; coinCount--)
        {
            int pow = Mathf.RoundToInt(Mathf.Pow(10, coinCount)); // 10^n 을 int로

            if (coinSum >= pow)
            {
                int divideResult = coinSum / pow;

                for (int i = 0; i < divideResult; i++) // 나눈거만큼 반복(3000/1000 = 3 나오면 3번 반복)
                {
                    coinCreateList.Add(coinSprites[coinCount]);
                }

                coinSum -= divideResult * pow;
            }
        }

        for (int j = 0; j < coinCreateList.Count; j++)
        {
            float theta = (360f / coinCreateList.Count) * j;
            GameObject coinObj = coinObjList.Find(x => !x.activeSelf);

            if (coinObj == null)
            {
                coinObj = Instantiate(coinPrefab, coinObjParent.transform);
                coinObjList.Add(coinObj);
            }

            coinObj.transform.position = centerPos;
            coinObj.GetComponent<Image>().sprite = coinCreateList[j];

            coinObj.SetActive(true);

            Vector3 movePos = Vector3.zero;
            movePos.x = Mathf.Cos(theta * Mathf.Deg2Rad) * R + centerPos.x;
            movePos.y = Mathf.Sin(theta * Mathf.Deg2Rad) * R + centerPos.y;
            coinObj.transform.DOMove(movePos, 0.25f);
        }

        //for (int j = 0; j < amount; j++)
        //{
        //    float theta = (360f / amount) * j;
        //    GameObject coinObj = coinObjList.Find(x => !x.activeSelf);

        //    if (coinObj == null)
        //    {
        //        coinObj = Instantiate(coinPrefab, coinObjParent.transform);
        //        coinObjList.Add(coinObj);
        //    }

        //    coinObj.transform.position = centerPos;

        //    coinObj.SetActive(true);

        //    Vector3 movePos = Vector3.zero;
        //    movePos.x = Mathf.Cos(theta * Mathf.Deg2Rad) * R + centerPos.x;
        //    movePos.y = Mathf.Sin(theta * Mathf.Deg2Rad) * R + centerPos.y;
        //    coinObj.transform.DOMove(movePos, 0.25f);
        //}
    }

    public void GetAllCoin()
    {
        StartCoroutine(GetCoinCor());
    }

    private IEnumerator GetCoinCor()
    {
        originGold = goldValue.RuntimeValue;

        // 돈 들어오는 연출
        foreach (GameObject coinObj in coinObjList)
        {
            if (coinObj.activeSelf == true)
            {
                CoinMove(coinObj);
            }
        }

        yield return new WaitForSeconds(1f);

        AddScore(allCoinAmount);
        allCoinAmount = 0;

        ShowResult();
    }

    private void CoinMove(GameObject coinObj)
    {
        coinObj.transform.DOMove(goldTextObj.transform.position, 0.5f).OnComplete( () =>
        {
        
            coinObj.SetActive(false);
        });
    }

    private void AddScore(int amount)
    {
        goldValue.RuntimeValue += amount;
        GoldChangeEvent.Occurred();
    }

    public void OnClickFieldResetEnd()
    {
        HideResult();

        FieldResetAfterEvnet.Occurred();
    }

    public void ShowResult()
    {
        // 결과창 띄우기
        int earnGold = goldValue.RuntimeValue - originGold;
        originText.text = originGold.ToString();
        earnText.text = earnGold.ToString();
        resultText.text = goldValue.RuntimeValue.ToString();

        resultPanel.SetActive(true);
    }

    public void HideResult()
    {
        resultPanel.SetActive(false);
    }
}