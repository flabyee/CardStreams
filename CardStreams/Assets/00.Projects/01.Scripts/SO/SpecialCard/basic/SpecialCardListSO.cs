using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialCardListSO", menuName = "ScriptableObject/SpecialCard/SpecialCardListSO")]
public class SpecialCardListSO : ScriptableObject
{
    public List<SpecialCardSO> specialCardList;

    //public List<BuildSO> commonList;
    //public List<BuildSO> rareList;
    //public List<BuildSO> epicList;
    //public List<BuildSO> uniqueList;
    //public List<BuildSO> legendarayList;
}
