using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Build : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuildSO buildSO;

    public GameObject areaTooltipPrefab;

    public Image buildImage;
    public TextMeshProUGUI greadeText;
    public GameObject buildAreaTooltip;
    private List<Vector2> accessPointList;
    private List<Image> buildAreaImageList = new List<Image>();
    private float width;
    private float height;

    private Vector2 myPoint;    // �ʿ����� �� ��ġ

    private ActionPosData actionPosData;

    protected virtual void Awake()
    {

        HideArea();
    }

    private void Start()
    {

    }

    public void Init(BuildSO buildSO)
    {
        this.buildSO = buildSO;

        buildImage.sprite = this.buildSO.sprite;
        greadeText.text = this.buildSO.grade.ToString();
        greadeText.color = ConstManager.Instance.gradeColorDict[this.buildSO.grade];

        width = GetComponent<RectTransform>().rect.width;
        height = width;

        //for (int i = 0; i < 9; i++)
        //{
        //    if (!(buildSO.areaList.Contains(i)))
        //        buildAreaTooltip.transform.GetChild(i).gameObject.GetComponent<Image>().DOFade(0, 0);
        //}

        accessPointList = buildSO.accessPointList;
        for (int y = 2; y >= -2; y--)
        {
            for (int x = -2; x <= 2; x++)
            {
                GameObject obj = Instantiate(areaTooltipPrefab, buildAreaTooltip.transform);

                // point�� �ش�Ǵ� �����̾ƴ϶�� Alpha = 0
                if(!buildSO.accessPointList.Contains(new Vector2(x, y)))
                {
                    Image image = obj.GetComponent<Image>();
                    image.color = new Color(0, 0, 0, 0);
                }
            }
        }
    }

    public void BuildDrop(Vector2 point)
    {
        myPoint = point;
        // �ֺ� �˻��ؼ� ȿ�� ����

        foreach (Vector2 accessPoint in buildSO.accessPointList)
        {

            // mapRectArr�� x�� ���ʿ��� ������������, y�� ������ �Ʒ��� + ��� -�� �ߴ�
            Field field = MapManager.Instance.mapRectArr[
                Mathf.Clamp(Mathf.RoundToInt(myPoint.y - accessPoint.y), 0, 9),
                Mathf.Clamp(Mathf.RoundToInt(myPoint.x + accessPoint.x), 0, 9)].GetComponent<Field>();
            if(field != null)
            {
                field.accessBuildToPlayerAfterOnField += buildSO.AccessPlayer;
                field.accessBuildToCardAfterMoveStart += buildSO.AccessCard;
            }
        }


        // �� ���� ȿ�� ���� ����Ʈ�� �߰� (���� : ������ �ǹ�)
        //BuildManager.Instance.OnBuildWhenTurnEnd += buildSO.AccessTurnEnd;
        actionPosData = new ActionPosData(buildSO.AccessTurnEnd, gameObject);
        BuildManager.Instance.OnBuildWhenTurnEndList.Add(actionPosData);
    }

    public void BuildUp(Vector2 point)
    {
        myPoint = point;
        // �ֺ� �˻��ؼ� ȿ�� ����

        foreach (Vector2 accessPoint in buildSO.accessPointList)
        {

            // mapRectArr�� x�� ���ʿ��� ������������, y�� ������ �Ʒ��� + ��� -�� �ߴ�
            Field field = MapManager.Instance.mapRectArr[
                Mathf.Clamp(Mathf.RoundToInt(myPoint.y - accessPoint.y), 0, 9),
                Mathf.Clamp(Mathf.RoundToInt(myPoint.x + accessPoint.x), 0, 9)].GetComponent<Field>();
            if (field != null)
            {
                field.accessBuildToPlayerAfterOnField -= buildSO.AccessPlayer;
                field.accessBuildToCardAfterMoveStart -= buildSO.AccessCard;
            }
        }

        BuildManager.Instance.OnBuildWhenTurnEndList.Remove(actionPosData);
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
        BuildTooltip.Instance.Show(buildSO.buildName, accessPointList, buildSO.tooltip, buildSO.sprite, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuildTooltip.Instance.Hide();
    }
}
