using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagController : MonoBehaviour
{
    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    public GameObject bagCardPrefab;
    public RectTransform buildScrollTrm;
    public RectTransform specialScrollTrm;

    public GameObject buildPanel;
    public GameObject specialPanel;

    public void Show()
    {
        ClearScroll();

        Debug.Log("dsaf");
        gameObject.SetActive(true);

        int count = 0;
        // 카드 생성
        foreach (int id in GameManager.Instance.handleController.GetBuildDeck())
        {
            CreateCard(CardType.Build, id);
            count++;
            if(count == 5)
            {
                count = 1;

                buildScrollTrm.sizeDelta += new Vector2(0, 280);
            }
        }

        count = 0;
        foreach (int id in GameManager.Instance.handleController.GetSpecialDeck())
        {
            CreateCard(CardType.Special, id);
            count++;
            if (count == 5)
            {
                count = 1;

                specialScrollTrm.sizeDelta += new Vector2(0, 280);
            }
        }

        OnBuild();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnBuild()
    {
        buildPanel.SetActive(true);
        specialPanel.SetActive(false);
    }

    public void OnSpecial()
    {
        specialPanel.SetActive(true);
        buildPanel.SetActive(false);
    }

    private void CreateCard(CardType cardType, int id)
    {
        GameObject card;
        if(poolQueue.Count > 0)
        {
            card = poolQueue.Dequeue();
        }
        else
        {
            card = Instantiate(bagCardPrefab, buildScrollTrm);
        }

        BagCard bagCard = card.GetComponent<BagCard>();

        if(cardType == CardType.Build)
        {
            card.transform.SetParent(buildScrollTrm);

            BuildSO buildSO = DataManager.Instance.GetBuildSO(id);
            bagCard.Init(buildSO.buildName, buildSO.tooltip, buildSO.sprite, CardType.Build);
        }
        else
        {
            card.transform.SetParent(specialScrollTrm);

            SpecialCardSO specialSO = DataManager.Instance.GetSpecialCardSO(id);
            bagCard.Init(specialSO.specialCardName, specialSO.tooltip, specialSO.sprite, CardType.Build);
        }
    }

    public void OnClickExit()
    {
        ClearScroll();

        Hide();
    }

    private void ClearScroll()
    {
        foreach (RectTransform item in buildScrollTrm)
        {
            poolQueue.Enqueue(item.gameObject);
        }
        foreach (RectTransform item in specialScrollTrm)
        {
            poolQueue.Enqueue(item.gameObject);
        }

        buildScrollTrm.sizeDelta = new Vector2(0, 265);
        specialScrollTrm.sizeDelta = new Vector2(0, 265);
    }
}
