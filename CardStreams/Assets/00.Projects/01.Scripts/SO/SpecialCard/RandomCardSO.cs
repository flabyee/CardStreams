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
        int addAmount = cardPower.value % 2 != 0 ? changeAmount : -changeAmount; // Ȧ��+ ¦��-
        addAmount = Mathf.Clamp(addAmount, -cardPower.value, int.MaxValue); // 2���Ϳ� -4�ֻ����� ������ 0�̵ǰ��Ϸ��� (-ī���ġ) ������ ����������
        cardPower.AddValue(addAmount);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}