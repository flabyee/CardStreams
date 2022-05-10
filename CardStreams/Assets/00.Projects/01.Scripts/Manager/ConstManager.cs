using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstManager : MonoBehaviour
{
    public static ConstManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("sprite")]
    public Sprite swordSprite;
    public Sprite sheildSprite;
    public Sprite potionSprite;
    public Sprite monsterSprite;
    public Sprite coinSprite;
}
