using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BasicType
{
    Sword,
    Sheild,
    Potion,
    Monster,
}

public class BasicCard : CardPower, IPointerClickHandler
{
    public BasicType basicType;
    public int value { get; private set; }
    public int goldP;   // 몬스터를 잡을 때 얻는 골드 배율, 기본 1
    public int originValue;

    // public List<BuffSO> buffList = new List<BuffSO>();
    public List<Buff> buffList = new List<Buff>();

    public override void SetData_Feild(BasicType basicType, int value)
    {
        base.SetData_Feild(basicType, value);

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


        switch (basicType)
        {
            case BasicType.Potion:
                faceImage.sprite = ConstManager.Instance.potionSprite[tempValue];
                break;
            case BasicType.Sword:
                faceImage.sprite = ConstManager.Instance.swordSprite[tempValue];
                break;
            case BasicType.Sheild:
                faceImage.sprite = ConstManager.Instance.sheildSprite[tempValue];
                break;
            case BasicType.Monster:
                faceImage.sprite = ConstManager.Instance.monsterSprite[tempValue];
                break;
        }

        valueText.text = value.ToString();

        if (value == originValue)
        {
            valueText.color = Color.white;
        }
        else if (value > originValue)
        {
            valueText.color = Color.blue;
        }
        else if (value < originValue)
        {
            valueText.color = Color.red;
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
        GameManager.Instance.DropField(GetComponent<DragbleCard>());
    }
}
