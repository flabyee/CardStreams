using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OnFieldTooltip : MonoBehaviour
{
    public static OnFieldTooltip Instance;

    private Image image;

    private void Awake()
    {
        Instance = this;

        image = GetComponent<Image>();

        Hide();
    }

    public void ShowCard(Vector3 pos, BasicCard cardPower)
    {
        transform.position = pos;

        image.DOFade(1, 0f);

        switch(cardPower.basicType)
        {
            case BasicType.Potion:
                image.sprite = ConstManager.Instance.potionSprite;
                break;
            case BasicType.Monster:
                int tempValue = Mathf.Clamp(cardPower.originValue, 0, ConstManager.Instance.monsterSprite.Length - 1);
                image.sprite = ConstManager.Instance.monsterSprite[tempValue];
                break;
            case BasicType.Sword:
                image.sprite = ConstManager.Instance.swordSprite;
                break;
            case BasicType.Sheild:
                image.sprite = ConstManager.Instance.shieldSprite;
                break;
        }

        gameObject.SetActive(true);

        transform.DOMove(pos + new Vector3(0, 1, 0), 0.2f);
        image.DOFade(0, 0.2f);
    }

    public void ShowBuild(Vector3 pos, Sprite sprite)
    {
        transform.position = pos;

        image.DOFade(1, 0f);

        image.sprite = sprite;

        gameObject.SetActive(true);

        transform.DOMove(pos + new Vector3(0, 1, 0), 1f);
        image.DOFade(0, 1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
