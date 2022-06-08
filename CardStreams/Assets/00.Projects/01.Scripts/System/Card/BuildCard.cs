using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BuildCard : CardPower, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI greadeText;

    public BuildSO buildSO;
    private List<Vector2> accessPointList;

    public bool isDrop;

    private Vector2 myPoint;    // �ʿ����� �� ��ġ

    private ActionPosData actionPosData;

    protected virtual void Awake()
    {
    }

    private void Start()
    {

    }

    public void Init(BuildSO buildSO)
    {
        this.buildSO = buildSO;

        faceImage.sprite = this.buildSO.sprite;
        greadeText.text = this.buildSO.grade.ToString();
        accessPointList = this.buildSO.accessPointList;
        
        greadeText.color = ConstManager.Instance.gradeColorDict[this.buildSO.grade];

        isDrop = false;
    }

    public void BuildDrop(Vector2 point)
    {
        greadeText.text = string.Empty;

        isDrop = true;

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

    public void BuildUp()
    {
        isDrop = false;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isDrop == true)
        {
            BuildAreaTooltip.Instance.Show(transform.position, accessPointList);
            BuildExplain.Instance.Show(buildSO);
        }
        else
        {
            BuildTooltip.Instance.Show(buildSO.buildName, accessPointList, buildSO.tooltip, buildSO.sprite, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isDrop == true)
        {
            BuildAreaTooltip.Instance.Hide();
            BuildExplain.Instance.Hide();
        }
        else
        {
            BuildTooltip.Instance.Hide();
        }
    }

    public List<Vector2> GetAccessPointList()
    {
        return accessPointList;
    }
}
