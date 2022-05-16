using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryPotion", menuName = "ScriptableObject/SpecialCard/RecoveryPotion")]
public class RecoveryPotionCardSO : SpecialCardSO
{
    [Header("���� SO")]
    public RecoveryBuffSO recoverySO;

    public override void AccessSpecialCard(Player player, Field field)
    {
        // cardPower

        Buff buff = new Buff();
        recoverySO.Init(buff); // SO�� ������ Buff�� �ʱ�ȭ����
        field.cardPower.AddBuff(buff);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
