using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseUITooltip_EquipMenu_UpgradeBtn : UseUITooltip
{
    public EquipController equipController;

    private void Start()
    {
        SetTooltip();
        equipController.UpgardeRemoveCountEvent += SetTooltip;
    }

    private void SetTooltip()
    {
        msg = $"업그레이드하면 제외 갯수가 늘어납니다(비용: {equipController.GetUpgradePrice()} 크리스탈)";

        UITooltip.Instance.UpdateText(msg);
    }
}
