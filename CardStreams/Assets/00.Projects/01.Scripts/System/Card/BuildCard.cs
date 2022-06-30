using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BuildCard : CardPower, IPointerEnterHandler, IPointerExitHandler
{
    public BuildSO buildSO;
    private List<Vector2> accessPointList;

    public bool isDrop;

    private Vector2 myPoint;    // 맵에서의 내 위치

    private ActionPosData actionPosData;


    public void Init(BuildSO buildSO)
    {
        this.buildSO = buildSO;

        faceImage.sprite = this.buildSO.sprite;
        fieldImage.sprite = this.buildSO.sprite;
        //greadeText.text = this.buildSO.grade.ToString();
        accessPointList = this.buildSO.accessPointList;
        
        //greadeText.color = ConstManager.Instance.gradeColorDict[this.buildSO.grade]; // 이거 Dictionary라서 작동을 안하는데용?? Hierarchy에서 못바꾸는거라

        isDrop = false;
    }

    public void BuildDrop(Vector2 point)
    {
        BuildManager.Instance.buildList.Add(this);

        //greadeText.text = string.Empty;

        isDrop = true;

        myPoint = point;
        // 주변 검사해서 효과 적용

        foreach (Vector2 accessPoint in buildSO.accessPointList)
        {

            // mapRectArr이 x는 왼쪽에서 오른쪽이지만, y는 위에서 아래라서 + 대신 -를 했다
            Field field = MapManager.Instance.mapRectArr[
                Mathf.Clamp(Mathf.RoundToInt(myPoint.y - accessPoint.y), 0, 9),
                Mathf.Clamp(Mathf.RoundToInt(myPoint.x + accessPoint.x), 0, 9)].GetComponent<Field>();
            if(field != null)
            {
                field.accessBuildToPlayerAfterOnField += buildSO.AccessPlayer;
                field.accessBuildToCardAfterMoveStart += buildSO.AccessCard;
            }
        }


        // 턴 엔드 효과 실행 리스트에 추가 (예시 : 돈버는 건물)
        //BuildManager.Instance.OnBuildWhenTurnEnd += buildSO.AccessTurnEnd;
        actionPosData = new ActionPosData(buildSO.AccessTurnEnd, gameObject);
        BuildManager.Instance.OnBuildWhenTurnEndList.Add(actionPosData);
    }

    public void BuildUp()
    {
        BuildManager.Instance.buildList.Remove(this);

        isDrop = false;

        // 주변 검사해서 효과 빼기
        foreach (Vector2 accessPoint in buildSO.accessPointList)
        {

            // mapRectArr이 x는 왼쪽에서 오른쪽이지만, y는 위에서 아래라서 + 대신 -를 했다
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
            HandleCardTooltip.Instance.ShowBuild(transform.position + transform.up * 0.5f, faceImage.sprite, buildSO.buildName, buildSO.tooltip);

            // BuildTooltip.Instance.Show(buildSO.buildName, accessPointList, buildSO.tooltip, buildSO.sprite, transform.position);
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
            HandleCardTooltip.Instance.Hide();

            // BuildTooltip.Instance.Hide();
        }
    }

    public List<Vector2> GetAccessPointList()
    {
        return accessPointList;
    }

    public Vector2 GetMyPoint()
    {
        return myPoint;
    }
}
