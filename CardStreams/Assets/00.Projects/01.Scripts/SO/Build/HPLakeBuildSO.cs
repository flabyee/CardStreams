using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HPLakeBuild", menuName = "ScriptableObject/Build/HPLakeBuild")]
public class HPLakeBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue hpValue;

    public EventSO playerValueChangeEvnet;

    [Header("Amount")]
    public int healAmount;

    public override void AccessCard(Field field)
    {

    }

    public override void AccessPlayer(Player player)
    {
        if (hpValue.RuntimeValue <= 6)
        {
            hpValue.RuntimeValue += healAmount;

            playerValueChangeEvnet.Occurred();
            OnFieldTooltip.Instance.ShowBuild(player.transform.position, sprite);
        }
    }
}
