using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSlot : MonoBehaviour
{
    [SerializeField] Image passiveImage;
    [SerializeField] TextMeshProUGUI passiveLevelText;
    [SerializeField] TextMeshProUGUI passiveNameText;

    private const string levelText = "Lv";

    public void Init(Sprite passiveSprite)
    {
        passiveImage.sprite = passiveSprite;
        passiveLevelText.text = 1 + levelText;
    }

    public void Init(Sprite icon, string passiveName, int level)
    {
        passiveImage.sprite = icon;
        passiveLevelText.text = passiveName;
        passiveNameText.text = level + levelText;
    }

    public void LevelUpdate(int level)
    {
        passiveLevelText.text = level + levelText;
    }

    // (PassiveSO��°� ���� ���� ���������� BuffSO ��¼����¼��orBuffSO ������ġ
    // �нú� �ǹ��� �߰��ϱ�
}
