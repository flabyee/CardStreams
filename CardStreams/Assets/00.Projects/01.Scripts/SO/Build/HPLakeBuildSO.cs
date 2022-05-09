using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HPLakeBuild", menuName = "ScriptableObject/Build/HPLakeBuild")]
public class HPLakeBuildSO : BuildSO
{
    public override void AccessCard(Field field)
    {

    }

    public override void AccessPlayer(Player player)
    {
        if (hpValue.RuntimeValue <= 6)
        {
            hpValue.RuntimeValue += 1;

            playerValueChangeEvnet.Occurred();
            OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
        }

    }
}
