using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EarnGoldBuild", menuName = "ScriptableObject/Build/EarnGoldBuild")]
public class EarnGoldBuildSO : BuildSO
{
    public override void AccessTurnEnd()
    {
        goldValue.RuntimeValue += 5;

        goldChangeEvnet.Occurred();
    }
}
