using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BleedSwordCard", menuName = "ScriptableObject/SpecialCard/BleedSwordCard")]
public class BleedSwordCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int upSwordAmount;

    [Header("버프 SO")]
    public ReduceHPDebuffSO reduceHPso;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        player.playerValueChangeEvent.Occurred();

        // Buff Add
        Buff buff = new Buff();
        reduceHPso.Init(buff); // SO의 값으로 Buff를 초기화해줌
        cardPower.AddBuff(buff);

        // cardPower Add
        cardPower.AddValue(upSwordAmount);

        // Show icon
        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    } 
}