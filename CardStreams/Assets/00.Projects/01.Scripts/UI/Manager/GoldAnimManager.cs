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

    private int originGold;

    public IntValue goldValue;
    public EventSO FieldResetAfterEvnet;
    public EventSO GoldChangeEvent;

    [SerializeField] Sprite[] coinSprites; // 1 10 100 ���� Sprite
    private int allCoinAmount;

    [SerializeField] private float R = 0.5f;

    private void Awake()
    {
        Instance = this;

        HideResult();
    }

    // �����
    // CreateCoin�� ���� ��, ��ġ�� ������ ������ �����ϰ� GetAllCoin �����ؼ� ������ ������ ��� ����ϰ� ���� ��´�

    public void CreateCoin(int amount, Vector3 centerPos, bool allCollect = true)
    {
        SoundManager.Instance.PlaySFX(SFXType.CreateMoney);

        // coin ����
        allCoinAmount += amount;

        int coinSum = amount;
        List<Sprite> coinCreateList = new List<Sprite>();
        List<GameObject> coinList = new List<GameObject>();

        for (int coinCount = coinSprites.Length - 1; coinCount >= 0; coinCount--)
        {
            int pow = Mathf.RoundToInt(Mathf.Pow(10, coinCount)); // 10^n �� int��

            if (coinSum >= pow)
            {
                int divideResult = coinSum / pow;

                for (int i = 0; i < divideResult; i++) // �����Ÿ�ŭ �ݺ�(3000/1000 = 3 ������ 3�� �ݺ�)
                {
                    coinCreateList.Add(coinSprites[coinCount]);

                }

                coinSum -= divideResult * pow;
            }
        }

        for (int j = 0; j < coinCreateList.Count; j++)
        {
            //float theta = (360f / coinCreateList.Count) * j;
            float theta = (360f / coinCreateList.Count) * j;
            GameObject coinObj = coinObjList.Find(x => !x.activeSelf);

            if (coinObj == null)
            {
                coinObj = Instantiate(coinPrefab, coinObjParent.transform);
                coinObjList.Add(coinObj);
            }

            coinList.Add(coinObj);

            coinObj.transform.position = centerPos;
            coinObj.GetComponent<Image>().sprite = coinCreateList[j];

            coinObj.SetActive(true);

            Vector3 movePos = Vector3.zero;
            movePos.x = Mathf.Cos(theta * Mathf.Deg2Rad) * R + centerPos.x;
            movePos.y = Mathf.Sin(theta * Mathf.Deg2Rad) * R + centerPos.y;
            coinObj.transform.DOMove(movePos, 0.25f);
        }

        if (allCollect)
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(1f);
            seq.AppendCallback(() => GetAllCoin());
        }
    }

    public void GetAllCoin(bool isJungsan = false)
    {
        StartCoroutine(GetCoinCor(isJungsan));
    }

    private IEnumerator GetCoinCor(bool isJungsan)
    {
        originGold = goldValue.RuntimeValue;

        // �� ������ ����
        foreach (GameObject coinObj in coinObjList)
        {
            if (coinObj.activeSelf == true)
            {
                CoinMove(coinObj);
            }
        }

        yield return new WaitForSeconds(0.5f);

        AddScore(allCoinAmount);
        allCoinAmount = 0;

        if(isJungsan == true)
        {
            ShowResult();
        }
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

    private void ShowResult()
    {
        // ���â ����
        GameManager.Instance.blurController.SetActive(true);
        int earnGold = goldValue.RuntimeValue - originGold;
        originText.text = originGold.ToString();
        earnText.text = earnGold.ToString();
        resultText.text = goldValue.RuntimeValue.ToString();

        resultPanel.SetActive(true);
    }

    private void HideResult()
    {
        resultPanel.SetActive(false);
        GameManager.Instance.blurController.SetActive(false);
    }

    public void OnClickCloseResultPanle()
    {
        HideResult();
    }
}