using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum BasicType
{
    NULL,
    Sword,
    Sheild,
    Potion,
    Monster,
}

public class BasicCard : CardPower, IPointerClickHandler
{
    public BasicType basicType;



    public Image backColorImage;

    public TextMeshProUGUI valueText;
    public Image fieldImage;
    public TextMeshProUGUI fieldText;

    public int value { get; private set; }
    public int goldP;   // 몬스터를 잡을 때 얻는 골드 배율, 기본 1
    public int originValue;

    // public List<BuffSO> buffList = new List<BuffSO>();
    public List<Buff> buffList = new List<Buff>();

    public override void InitData_Feild(BasicType basicType, int value)
    {
        base.InitData_Feild(basicType, value);

        this.value = value;
        this.originValue = value;
        this.basicType = basicType;
        this.goldP = 1;

        ApplyUI();
    }

    public void AddBuff(Buff buff)
    {
        if (buffList.Contains(buff)) return; // 대충 중복체크해서 없으면넣기
        // 나중엔 중복되는버프 들어가게해줘야함

        buffList.Add(buff);
    }

    public void ApplyUI()
    {
        int tempValue = Mathf.Clamp(value, 0, ConstManager.Instance.potionSprite.Length - 1);

        backColorImage.color = ConstManager.Instance.basicTypeColorList[(int)basicType];

        switch (basicType)
        {
            case BasicType.Potion:
                faceImage.sprite = ConstManager.Instance.potionSprite[tempValue];
                fieldImage.sprite = ConstManager.Instance.potionSprite[tempValue];
                break;
            case BasicType.Sword:
                faceImage.sprite = ConstManager.Instance.swordSprite[tempValue];
                fieldImage.sprite = ConstManager.Instance.swordSprite[tempValue];
                break;
            case BasicType.Sheild:
                faceImage.sprite = ConstManager.Instance.sheildSprite[tempValue];
                fieldImage.sprite = ConstManager.Instance.sheildSprite[tempValue];
                break;
            case BasicType.Monster:
                faceImage.sprite = ConstManager.Instance.monsterSprite[tempValue];
                fieldImage.sprite = ConstManager.Instance.monsterSprite[tempValue];
                break;
        }

        valueText.text = value.ToString();
        fieldText.text = value.ToString();

        if (value == originValue)
        {
            valueText.color = Color.white;
            fieldText.color = Color.white;
        }
        else if (value > originValue)
        {
            valueText.color = ConstManager.Instance.upValueColor;
            fieldText.color = ConstManager.Instance.upValueColor;
        }
        else if (value < originValue)
        {
            valueText.color = ConstManager.Instance.downValueColor;
            fieldText.color = ConstManager.Instance.downValueColor;
        }
    }

    public void AddValue(int value)
    {
        this.value += value;
        ApplyUI();
    }

    public void SetValue(int value)
    {
        this.value = value;
        ApplyUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isHandle && eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.Instance.DropField(GetComponent<DragbleCard>());
        }
    }

    public override void OnHandle()
    {
        base.OnHandle();

        ApplyUI();
    }
    public override void OnField()
    {
        base.OnField();

        ApplyUI();
    }
    public override void OnHover()
    {
        base.OnHover();

        ApplyUI();
    }
}
