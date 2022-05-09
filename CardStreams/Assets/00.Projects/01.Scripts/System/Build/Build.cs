using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Build : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuildSO buildSO;

    private Image buildImage;
    private GameObject buildAreaTooltip;
    private List<Image> buildAreaImageList = new List<Image>();
    private float width;
    private float height;

    private Vector2 myPoint;    // 맵에서의 내 위치

    protected virtual void Awake()
    {
        buildImage = transform.Find("BuildImage").GetComponent<Image>();
        buildAreaTooltip = transform.Find("BuildAreaTooltip").gameObject;

        HideArea();
    }

    private void Start()
    {

    }

    public void Init(BuildSO buildSO)
    {
        this.buildSO = buildSO;

        buildImage.sprite = this.buildSO.sprite;

        width = GetComponent<RectTransform>().rect.width;
        height = width;

        //for (int i = 0; i < 9; i++)
        //{
        //    if (!(buildSO.areaList.Contains(i)))
        //        buildAreaTooltip.transform.GetChild(i).gameObject.GetComponent<Image>().DOFade(0, 0);
        //}

        foreach (Vector2 point in buildSO.accessPointList)
        {

        }
    }

    public void BuildDrop()
    {
        // 주변 검사해서 효과 적용
        //StartCoroutine(NearbyFieldSearchCor());

        // 턴 엔드 효과 실행 리스트에 추가 (예시 : 돈버는 건물)
        BuildManager.Instance.OnBuildWhenTurnEnd += buildSO.AccessTurnEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Field field = collision.GetComponent<Field>();
        if (field != null)
        {
            Debug.Log("access");
            field.accessBuildToPlayerAfterOnField += buildSO.AccessPlayer;
            field.accessBuildToCardAfterMoveStart += buildSO.AccessCard;
        }
    }

    //IEnumerator NearbyFieldSearchCor()
    //{
    //    int index = 0;
    //    for (int y = 1; y >= -1; y--)
    //    {
    //        for (int x = -1; x <= 1; x++)
    //        {
    //            if (buildSO.areaList.Contains(index))
    //            {
    //                boxCollider.offset = new Vector2(width * x, height * y);
    //                boxCollider.enabled = true;
    //                yield return new WaitForSeconds(0.1f);
    //            }
    //            boxCollider.enabled = false;
    //            index++;

    //        }
    //    }
    //}

    public void ShowArea()
    {
        buildAreaTooltip.SetActive(true);
    }

    public void HideArea()
    {
        buildAreaTooltip.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        CardTooltip.Instance.Show(buildSO.buildName, buildSO.tooltip, buildSO.sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardTooltip.Instance.Hide();
    }
}
