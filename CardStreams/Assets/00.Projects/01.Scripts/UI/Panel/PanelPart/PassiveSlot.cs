using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSlot : MonoBehaviour
{
    [SerializeField] Image passiveImage;
    [SerializeField] TextMeshProUGUI passiveLevelText;

    public void Init(Sprite passiveSprite)
    {
        passiveImage.sprite = passiveSprite;
        passiveLevelText.text = 1 + "Lv";
    }

    public void LevelUpdate(int level)
    {
        passiveLevelText.text = level + "Lv";
    }

    // (PassiveSO��°� ���� ���� ���������� BuffSO ��¼����¼��orBuffSO ������ġ
    // �нú� �ǹ��� �߰��ϱ�
}
