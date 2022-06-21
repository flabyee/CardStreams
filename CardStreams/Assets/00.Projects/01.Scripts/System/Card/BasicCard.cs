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

public class BasicCard : CardPower, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public BasicType basicType;



    public Image backColorImage;

    public TextMeshProUGUI valueText;
    public TextMeshProUGUI fieldText;

    public int value { get; private set; }
    public int goldP;   // 몬스터를 잡을 때 얻는 골드 배율, 기본 1
    public int originValue;

    // public List<BuffSO> buffList = new List<BuffSO>();
    public List<Buff> buffList = new List<Buff>();

    private int originSiblingIndex; // 원래 자식 인덱스값(인덱스 순서기억)
    private Vector3 originPos; // 원래 위치(카드 커졌다가 다시돌아오기)
    private Quaternion originRot; // 원래 회전(카드 일직선이었다가 돌아오기)
    private void Start()
    {
        originSiblingIndex = transform.GetSiblingIndex();
    }

    public override void InitData_Feild(BasicType basicType, int value)
    {
        base.InitData_Feild(basicType, value);

        this.value = value;
        this.originValue = value;
        this.basicType = basicType;
        this.goldP = 2;

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
            transform.localScale = Vector3.one;
            GameManager.Instance.DropField(GetComponent<DragbleCard>());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHandle == false) return;

        originSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();

        originPos = transform.position;

        transform.position += transform.up * 0.5f;
        transform.localScale *= 1.5f;

        originRot = transform.rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHandle == false) return;

        transform.SetSiblingIndex(originSiblingIndex);

        transform.position = originPos;
        transform.localScale /= 1.5f;

        transform.rotation = originRot;
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
