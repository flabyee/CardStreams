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

        gradeColorDict = new Dictionary<CardGrade, Color>();
        gradeColorDict.Add(CardGrade.Common, Color.black);
        gradeColorDict.Add(CardGrade.Rare, Color.blue);
        gradeColorDict.Add(CardGrade.Epic, Color.magenta);
        gradeColorDict.Add(CardGrade.Unique, Color.yellow);
        gradeColorDict.Add(CardGrade.Legendary, Color.red);
    }

    [Header("sprite")]
    public Sprite[] swordSprite;
    public Sprite[] sheildSprite;
    public Sprite[] potionSprite;
    public Sprite[] monsterSprite;
    public Sprite[] coinSprite;

    public Sprite[] fieldSprites;

    public List<Color> basicTypeColorList;

    public Color upValueColor;
    public Color downValueColor;

    [Header("color")]
    public Dictionary<CardGrade, Color> gradeColorDict;
    public List<Sprite> gradeSpriteDict;
}
