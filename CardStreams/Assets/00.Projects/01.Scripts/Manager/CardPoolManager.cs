using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPoolManager : MonoBehaviour
{
    public static CardPoolManager Instance;

    public GameObject basicCardPrefab;
    public GameObject specialCardPrefab;
    public GameObject buildCardPrefab;

    private List<GameObject> basicCardList = new List<GameObject>();
    private List<GameObject> specialCardList = new List<GameObject>();
    private List<GameObject> buildCardList = new List<GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject GetBasicCard(Transform trm)
    {
        GameObject obj;
        obj = basicCardList.Find((x) => !x.activeSelf);
        if(obj == null)
        {
            obj = Instantiate(basicCardPrefab, trm);
            basicCardList.Add(obj);
        }
        obj.transform.SetParent(trm);
        obj.SetActive(true);

        return obj;
    }

    public GameObject GetSpecialCard(Transform trm)
    {
        GameObject obj;
        obj = specialCardList.Find((x) => !x.activeSelf);
        if (obj == null)
        {
            obj = Instantiate(specialCardPrefab, trm);
            specialCardList.Add(obj);
        }
        obj.transform.SetParent(trm);
        obj.SetActive(true);

        return obj;
    }

    public GameObject GetBuildCard(Transform trm)
    {
        GameObject obj;
        obj = buildCardList.Find((x) => !x.activeSelf);
        if (obj == null)
        {
            obj = Instantiate(buildCardPrefab, trm);
            buildCardList.Add(obj);
        }
        obj.transform.SetParent(trm);
        obj.SetActive(true);

        return obj;
    }
}
