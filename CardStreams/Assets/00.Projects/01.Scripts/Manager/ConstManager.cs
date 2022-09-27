using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstManager : MonoBehaviour
{
    public static ConstManager Instance;

    public RectTransform tempTrm;

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
    public Sprite shieldSprite;
    public Sprite potionSprite;
    public Sprite[] monsterSprite;
    public Sprite bossSprite;

    public Sprite goldSprite;
    public Sprite heartSprite;

    public Sprite[] expSprites;

    public Sprite[] fieldSprites;
    public Sprite[] nextFieldSprites;

    public Sprite[] missionImageSprites;

    public List<Color> basicTypeColorList;

    public Color buildColor;
    public Color specialCardColor;

    public Color upValueColor;
    public Color downValueColor;

    [Header("color")]
    public List<Color> gradeColorDict;
    public List<Sprite> gradeSpriteDict;
}
