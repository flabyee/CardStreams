using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstManager : MonoBehaviour
{
    public static ConstManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("sprite")]
    public Sprite swordSprite;
    public Sprite sheildSprite;
    public Sprite potionSprite;
    public Sprite monsterSprite;
    public Sprite coinSprite;
}
