using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarnGoldBuild", menuName = "ScriptableObject/Build/EarnGoldBuild")]
public class EarnGoldBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue goldValue;

    public EventSO goldChangeEvnet;

    [Header("Amount")]
    public int earnAmount;

    public override void AccessTurnEnd(Vector3 buildPos)
    {
        //goldValue.RuntimeValue += earnAmount;

        //goldChangeEvnet.Occurred();

        // GoldAnimManager.Instance.CreateCoin(earnAmount, buildPos);
        GoldAnimManager.Instance.CreateCoin(earnAmount, buildPos, false);
    }

}
