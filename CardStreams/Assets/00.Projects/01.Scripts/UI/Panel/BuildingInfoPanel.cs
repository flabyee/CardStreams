using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : Panel
{
    public Image[] buildingRangeImages;
    public TextMeshProUGUI buildTooltipText;
    public Image buildIconImage;

    private RectTransform _rectTrm;

    protected override void Awake()
    {
        base.Awake();
        _rectTrm = GetComponent<RectTransform>();
        Hide();
    }

    public void PanelUpdate(BuildSO so, Vector3 pos)
    {
        List<Vector2> s = so.accessPointList;

        for (int y =-1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector2 tilePos = new Vector2(x, y); // 타일의 위치
                Color tileColor = Color.red;

                if(s.Contains(tilePos)) // 범위 안에 있다!
                {
                    tileColor = Color.black;
                }
                else // 없다
                {
                    tileColor = Color.gray;
                }

                buildingRangeImages[(1-y) * 3 + x+1].color = tileColor;
            }
        }

        if(so is VillageNPCHouseSO)
        {
            buildTooltipText.text = (so as VillageNPCHouseSO).npcSO.rewardSO.rewardTooltip;
            buildIconImage.sprite = (so as VillageNPCHouseSO).npcSO.npcSprite;
        }
        else if(so is VillagePassiveBuildSO)
        {
            Debug.Log("passsve");
            buildTooltipText.text = (so as VillagePassiveBuildSO).passiveSO.buffTooltip;
            buildIconImage.sprite = (so as VillagePassiveBuildSO).passiveSO.buffIcon;
        }
        else
        {
            buildTooltipText.text = so.tooltip;
            buildIconImage.sprite = so.sprite;
        }

        Vector3 movePos = transform.position;
        movePos.y = Mathf.Clamp(pos.y, -3.5f, 3.7f);
        transform.position = movePos;
    }

    public void MovePanelX(float anchorX)
    {
        _rectTrm.anchoredPosition = new Vector2(anchorX, _rectTrm.anchoredPosition.y);
    }
}
