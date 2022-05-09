using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialCardListSO", menuName = "ScriptableObject/SpecialCard/SpecialCardListSO")]
public class SpecialCardListSO : ScriptableObject
{
    public List<SpecialCardSO> specialCardListSO;
}
