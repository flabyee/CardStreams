using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverHealPotion", menuName = "ScriptableObject/SpecialCard/OverHealPotion")]
public class OverHealPotionCardSO : SpecialCardSO
{
    public OverHealBuffSO overHealso;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        Debug.Log("OverHealBuffAdd");
        Buff buff = new Buff();
        overHealso.Init(buff); // SO�� ������ Buff�� �ʱ�ȭ����
        cardPower.AddBuff(buff);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
