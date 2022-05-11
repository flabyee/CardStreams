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

    public void ShowCard(Vector3 pos, CardType cardType)
    {
        transform.position = pos;

        image.DOFade(1, 0f);

        switch(cardType)
        {
            case CardType.Potion:
                image.sprite = ConstManager.Instance.potionSprite[0];
                break;
            case CardType.Coin:
                image.sprite = ConstManager.Instance.coinSprite[0];
                break;
            case CardType.Monster:
                image.sprite = ConstManager.Instance.monsterSprite[0];
                break;
            case CardType.Sword:
                image.sprite = ConstManager.Instance.swordSprite[0];
                break;
            case CardType.Sheild:
                image.sprite = ConstManager.Instance.sheildSprite[0];
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
