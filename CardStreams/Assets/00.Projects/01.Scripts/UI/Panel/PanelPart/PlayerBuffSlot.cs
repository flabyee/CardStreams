using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBuffSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public PlayerBuffPanel buffPanel;

    private Image slotImage;
    private Buff slotBuff;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buffPanel.ShowBuffInfo(slotBuff);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buffPanel.HideBuffInfo();
    }

    public void SetBuff(Buff buff)
    {
        if(buff == null) // 버프가 없다?
        {
            slotBuff = null;
            slotImage.sprite = null;
            gameObject.SetActive(false);
            return;
        }

        slotBuff = buff;
        slotImage.sprite = slotBuff.buffIcon;
        gameObject.SetActive(true);
    }

}
