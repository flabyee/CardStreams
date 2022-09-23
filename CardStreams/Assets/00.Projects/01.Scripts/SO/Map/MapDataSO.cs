using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new MapData", menuName = "ScriptableObject/Map/MapData")]
public class MapDataSO : ScriptableObject
{
    [TextArea(6, 6)]
    public string mapStr;
}
