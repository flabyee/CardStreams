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
        msg = $"���׷��̵��ϸ� ���� ������ �þ�ϴ�(���: {equipController.GetUpgradePrice()} ũ����Ż)";

        UITooltip.Instance.UpdateText(msg);
    }
}
