using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OverlayCard", menuName = "ScriptableObject/SpecialCard/OverlayCard")]
public class OverlayCardSO : SpecialCardSO
{
    [Header("���� SO")]
    public OverlayBuffSO overlaySO;

    public override void AccessBuildCard(Build build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        Debug.Log("AddBuffToField");
        Buff buff = new Buff();
        overlaySO.Init(buff); // SO�� ������ Buff�� �ʱ�ȭ����
        field.cardPower.AddBuff(buff);

        OnFieldTooltip.Instance.ShowBuild(field.transform.position, sprite);
    }
}
