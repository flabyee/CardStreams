using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VampireSword", menuName = "ScriptableObject/SpecialCard/VampireSword")]
public class VampireSwordCardSO : SpecialCardSO
{
    [Header("���� SO")]
    public LifeStealBuffSO lifeStealso;

    public override void AccessBuildCard(BuildCard build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        BasicCard cardPower = field.cardPower as BasicCard;

        Buff buff = new Buff();
        lifeStealso.Init(buff); // SO�� ������ Buff�� �ʱ�ȭ����
        cardPower.AddBuff(buff);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
