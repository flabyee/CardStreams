using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverlayCard", menuName = "ScriptableObject/SpecialCard/OverlayCard")]
public class OverlayCardSO : SpecialCardSO
{
    [Header("버프 SO")]
    public OverlayBuffSO overlaySO;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        Debug.Log("AddBuffToField");
        Buff buff = new Buff();
        overlaySO.Init(buff); // SO의 값으로 Buff를 초기화해줌
        cardPower.AddBuff(buff);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
