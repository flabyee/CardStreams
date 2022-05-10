using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarnGoldBuild", menuName = "ScriptableObject/Build/EarnGoldBuild")]
public class EarnGoldBuildSO : BuildSO
{
    [Header("Amount")]
    public int earnAmount;

    public override void AccessTurnEnd()
    {
        goldValue.RuntimeValue += earnAmount;

        goldChangeEvnet.Occurred();
    }

}
