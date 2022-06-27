using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockMenu : Menu<UnlockMenu>
{
    public UnlockController unlockController;

    public override void OnOpen()
    {
        unlockController.OnOpen();
    }

    public override void OnBackPressed()
    {
        unlockController.OnClose();

        base.OnBackPressed();
    }
}
