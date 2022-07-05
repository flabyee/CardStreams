using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomCard", menuName = "ScriptableObject/SpecialCard/RandomCard")]
public class RandomCardSO : SpecialCardSO
{
    [Header("amounts")]
    public int changeAmount;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        // cardPower
        int addAmount = cardPower.value % 2 != 0 ? changeAmount : -changeAmount; // 홀수+ 짝수-
        addAmount = Mathf.Clamp(addAmount, -cardPower.value, int.MaxValue); // 2몬스터에 -4주사위를 썼을때 0이되게하려고 (-카드수치) 까지만 내려가게함
        cardPower.AddValue(addAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}