using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowShield", menuName = "ScriptableObject/SpecialCard/ShadowShield")]
public class ShadowShieldCardSO : SpecialCardSO
{
    [Header("น๖วม SO")]

    int buffAddCount;

    public override void AccessBuildCard(Build build)
    {
    }

    public override void AccessSpecialCard(Player player, Field field)
    {
        
    }
}
