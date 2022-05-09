using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Build : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuildSO buildSO;

    public GameObject areaTooltipPrefab;

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

        for(int y = 2; y >= -2; y--)
        {
            for (int x = 2; x >= -2; x--)
            {
                Instantiate(areaTooltipPrefab, buildAreaTooltip.transform);
            }
        }
    }

    public void BuildDrop(Vector2 point)
    {
        myPoint = point;
        // 주변 검사해서 효과 적용

        foreach (Vector2 accessPoint in buildSO.accessPointList)
        {

            Field field = MapManager.Instance.mapRectArr[
                Mathf.Clamp(Mathf.RoundToInt(accessPoint.y + myPoint.y), 0, 10),
                Mathf.Clamp(Mathf.RoundToInt(accessPoint.x + myPoint.x), 0, 10)].GetComponent<Field>();
            if(field != null)
            {
                field.accessBuildToPlayerAfterOnField += buildSO.AccessPlayer;
                field.accessBuildToCardAfterMoveStart += buildSO.AccessCard;

                Debug.Log(field.dropArea.point);
            }
        }

        // 턴 엔드 효과 실행 리스트에 추가 (예시 : 돈버는 건물)
        BuildManager.Instance.OnBuildWhenTurnEnd += buildSO.AccessTurnEnd;
    }


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
        CardTooltip.Instance.Show(buildSO.buildName, null, buildSO.tooltip, buildSO.sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardTooltip.Instance.Hide();
    }
}
