using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryPotion", menuName = "ScriptableObject/SpecialCard/RecoveryPotion")]
public class RecoveryPotionCardSO : SpecialCardSO
{
    [Header("버프 SO")]
    public RecoveryBuffSO recoverySO;

    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower

        Buff buff = new Buff();
        recoverySO.Init(buff); // SO의 값으로 Buff를 초기화해줌
        field.cardPower.AddBuff(buff);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
