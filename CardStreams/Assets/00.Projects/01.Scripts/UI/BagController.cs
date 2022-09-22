using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagController : MonoBehaviour
{
    public GameObject bagCardPrefab;

    public RectTransform buildScrollTrm;
    public RectTransform specialScrollTrm;
    public GameObject notItemImage;

    public GameObject buildPanel;
    public GameObject specialPanel;

    public void Show()
    {
        ClearScroll();

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
        bool notItem = (buildScrollTrm.childCount <= 0) ? true : false;
        SetActiveNotItem(notItem);

        buildPanel.SetActive(true);
        specialPanel.SetActive(false);
    }

    public void OnSpecial()
    {
        bool notItem = (specialScrollTrm.childCount <= 0) ? true : false;
        SetActiveNotItem(notItem);

        specialPanel.SetActive(true);
        buildPanel.SetActive(false);
    }

    private void CreateCard(CardType cardType, int id)
    {
        GameObject obj = Instantiate(bagCardPrefab, buildScrollTrm);
        BagCard bagCard = obj.GetComponent<BagCard>();

        if(cardType == CardType.Build)
        {
            obj.transform.SetParent(buildScrollTrm);

            BuildSO buildSO = DataManager.Instance.GetBuildSO(id);
            bagCard.Init(buildSO.buildName, buildSO.tooltip, buildSO.sprite, CardType.Build);
        }
        else
        {
            obj.transform.SetParent(specialScrollTrm);

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
            Destroy(item.gameObject);
        }
        foreach (RectTransform item in specialScrollTrm)
        {
            Destroy(item.gameObject);
        }

        buildScrollTrm.sizeDelta = new Vector2(0, 290);
        specialScrollTrm.sizeDelta = new Vector2(0, 290);
    }

    public void SetActiveNotItem(bool value)
    {
        notItemImage.SetActive(value);
    }
}
