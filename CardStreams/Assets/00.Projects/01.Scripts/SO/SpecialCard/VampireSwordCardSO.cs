using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VampireSword", menuName = "ScriptableObject/SpecialCard/VampireSword")]
public class VampireSwordCardSO : SpecialCardSO
{
    public LifeStealBuffSO lifeStealso;

    public override void AccessSpecialCard(Player player, Field field)
    {
        field.cardPower.AddBuffSO(lifeStealso);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
