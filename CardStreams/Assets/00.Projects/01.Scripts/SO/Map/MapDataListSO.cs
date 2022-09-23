using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new MapData", menuName = "ScriptableObject/Map/MapList")]
public class MapDataListSO : ScriptableObject
{
    public List<MapDataSO> mapDataList;
}
