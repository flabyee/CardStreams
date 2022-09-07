using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMenu : Menu<EquipMenu>
{
    public GameObject buildPanel;
    public GameObject specialPanel;

    public EquipController equipController;

    public EventSO closeMenuEvent;

    public override void OnOpen()
    {
        equipController.OnEquip();

        OnClickBuildPanel();
    }

    public override void OnBackPressed()
    {
        equipController.Close();
        closeMenuEvent.Occurred();

        base.OnBackPressed();
    }

    public void OnClickBuildPanel()
    {
        buildPanel.SetActive(true);
        specialPanel.SetActive(false);
    }

    public void OnClickSpecialPanel()
    {
        specialPanel.SetActive(true);
        buildPanel.SetActive(false);
    }
}