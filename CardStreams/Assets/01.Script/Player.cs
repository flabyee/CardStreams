using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public EventSO playerValueChangeEvent;
    public IntValue hpValue;
    public IntValue swordValue;
    public IntValue shieldValue;

    private void Awake()
    {
       
    }

    void Start()
    {
        playerValueChangeEvent.Occurred();
    }

    public void OnFeild(Field field)
    {
        switch (field.cardType)
        {
            case CardType.Potion:
                hpValue.RuntimeValue = Mathf.Clamp(hpValue.RuntimeValue + field.value, 0, hpValue.RuntimeMaxValue);
                break;
            case CardType.Sword:
                swordValue.RuntimeValue = Mathf.Clamp(field.value, 0, swordValue.RuntimeMaxValue);
                break;
            case CardType.Sheild:
                shieldValue.RuntimeValue = Mathf.Clamp(field.value, 0, shieldValue.RuntimeMaxValue);
                break;
            case CardType.Monster:
                GameManager.Instance.AddScore(4);

                int damage = field.value;
                damage -= swordValue.RuntimeValue;
                swordValue.RuntimeValue = 0;

                if (damage > 0)
                {
                    int diff = shieldValue.RuntimeValue - damage;

                    if (diff > 0)
                    {
                        shieldValue.RuntimeValue -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= shieldValue.RuntimeValue;
                        shieldValue.RuntimeValue = 0;
                    }
                }

                damage = Mathf.Clamp(damage, 0, hpValue.RuntimeMaxValue);
                hpValue.RuntimeValue -= damage;

                break;
            case CardType.Coin:
                GameManager.Instance.AddScore(2);
                break;
            case CardType.Special:

            default:
                break;
        }

        playerValueChangeEvent.Occurred();

        OnFieldTooltip.Instance.ShowCard(transform.position, field.cardType);

    }

    public void Move(Vector3 pos, float duration)
    {
        pos.z = 0;
        transform.DOMove(pos, duration);
    }
}