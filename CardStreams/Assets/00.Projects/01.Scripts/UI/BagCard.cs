using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagCard : MonoBehaviour
{
    public CardType cardType;

    [Header("Build")]
    public GameObject build;
    public TextMeshProUGUI build_nameText;
    public TextMeshProUGUI build_infoText;
    public Image build_cardImage;

    [Header("Special")]
    public GameObject speical;
    public TextMeshProUGUI speical_nameText;
    public TextMeshProUGUI speical_infoText;
    public Image speical_cardImage;

    public void Init(string name, string info, Sprite sprite, CardType cardType)
    {
        if(cardType == CardType.Build)
        {
            build.SetActive(true);
            speical.SetActive(false);

            build_nameText.text = name;
            build_infoText.text = info;
            build_cardImage.sprite = sprite;
            this.cardType = cardType;
        }
        else
        {
            speical.SetActive(true);
            build.SetActive(false);

            speical_nameText.text = name;
            speical_infoText.text = info;
            speical_cardImage.sprite = sprite;
            this.cardType = cardType;
        }
    }

    public void OnClickBagCard()
    {

    }
}
