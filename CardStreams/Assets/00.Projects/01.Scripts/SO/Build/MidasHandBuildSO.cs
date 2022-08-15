using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MidasHand", menuName = "ScriptableObject/Build/MidasHand")]
public class MidasHandBuildSO : BuildSO
{
    [Header("SO")]
    public IntValue swordValue;
    public IntValue shieldValue;

    public EventSO playerValueChangeEvnet;
    public EventSO goldValueChangeEvent;

    [Header("Amount")]
    public int multiplyValue;

    public override void AccessCard(Field field)
    {
        
    }

    public override void AccessPlayer(Player player)
    {
        int sum = swordValue.RuntimeValue + shieldValue.RuntimeValue;
        Debug.Log(sum);

        GoldAnimManager.Instance.CreateCoin(sum * multiplyValue, player.transform.position, true);
        GoldAnimManager.Instance.GetAllCoin(1f, false);

        goldValueChangeEvent.Occurred();


        swordValue.RuntimeValue = 0;
        shieldValue.RuntimeValue = 0;

        playerValueChangeEvnet.Occurred();
    }
}
