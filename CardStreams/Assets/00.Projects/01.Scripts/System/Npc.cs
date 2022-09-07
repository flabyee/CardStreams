using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    private SpriteRenderer sr;
    private NpcSO npcSO;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(NpcSO so)
    {
        sr.sprite = so.npcSprite;
        npcSO = so;
    }

    public void AccessPlayer(Player player)
    {
        npcSO.AccessPlayer(player);
    }
}
