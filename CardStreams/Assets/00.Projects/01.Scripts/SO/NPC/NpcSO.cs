using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcSO : ScriptableObject
{
    public Sprite npcSprite;
    public string npcName;

    public abstract void AccessPlayer(Player player);
}
